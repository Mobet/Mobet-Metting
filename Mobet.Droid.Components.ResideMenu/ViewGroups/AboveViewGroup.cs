using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Views.Animations;
using Android.Util;


using Mobet.Droid.Components.ResideMenu.Events;
using Android.Graphics;
using Mobet.Droid.Components.ResideMenu.Enums;
using Android.Support.V4.View;

namespace Mobet.Droid.Components.ResideMenu.ViewGroups
{
    public delegate void ResidePageSelectedEventHandler(object sender, ResidePageSelectedEventArgs e);
    public delegate void ResidePageScrolledEventHandler(object sender, ResidePageScrolledEventArgs e);
    public delegate void ResidePageScrollStateChangedEventHandler(object sender, ResidePageScrollStateChangedEventArgs e);

    public class AboveViewGroup : ViewGroup
    {
        private new const string Tag = "AboveViewGroup";

        private const bool UseCache = false;

        private const int MaxSettleDuration = 600;  // ms
        private const int MinDistanceForFling = 25; // dips
        private const int InvalidPointer = -1;

        private readonly IInterpolator interpolator = new CVAInterpolator();
        private class CVAInterpolator : Java.Lang.Object, IInterpolator
        {
            public float GetInterpolation(float t)
            {
                t -= 1.0f;
                return t * t * t * t * t + 1.0f;
            }
        }

        private View content;
        private Scroller scroller;


        private bool scrollingCacheEnabled;
        private bool scrolling;
        private bool isBeingDragged;
        private bool isUnableToDrag;

        private float initialMotionX;
        private float lastMotionX;
        private float lastMotionY;

        private int curItem;
        private int touchSlop;
        private int minimumVelocity;
        private int flingDistance;
        private bool quickReturn;
        private bool enabled = true;

        private BehindViewGroup viewBehindGroup;
        private readonly IList<View> ignoredViews = new List<View>();
        private float scrollX;

        protected int ActivePointerId = InvalidPointer;
        protected int MaximumVelocity;
        protected VelocityTracker VelocityTracker;

        public ResidePageSelectedEventHandler ResidePageSelected;
        public ResidePageScrolledEventHandler ResidePageScrolled;
        public ResidePageScrollStateChangedEventHandler ResidePageScrollState;
        public EventHandler Closed;
        public EventHandler Opened;

        static float DistanceInfluenceForSnapDuration(float f)
        {
            f -= 0.5f;
            f *= 0.3f * (float)Math.PI / 2.0f;
            return FloatMath.Sin(f);
        }

        public AboveViewGroup(Context context) : this(context, null)
        {
        }
        public AboveViewGroup(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            InitViewAboveGroup();
        }

        public void SetCurrentItem(int item)
        {
            SetCurrentItemInternal(item, true, false);
        }
        public void SetCurrentItem(int item, bool smoothScroll)
        {
            SetCurrentItemInternal(item, smoothScroll, false);
        }
        public void AddIgnoredView(View v)
        {
            if (!ignoredViews.Contains(v))
                ignoredViews.Add(v);
        }
        public void RemoveIgnoredView(View v)
        {
            ignoredViews.Remove(v);
        }
        public void ClearIgnoredViews()
        {
            ignoredViews.Clear();
        }

        public int GetCurrentItem()
        {
            return curItem;
        }
        public int ContentLeft
        {
            get { return content.Left + content.PaddingLeft; }
        }
        public int GetDestScrollX(int page)
        {
            switch (page)
            {
                case 0:
                case 2:
                    return ViewBehind.GetMenuLeft(content, page);
                case 1:
                    return content.Left;
            }
            return 0;
        }
        public int BehindWidth
        {
            get
            {
                return ViewBehind == null ? 0 : ViewBehind.BehindWidth;
            }
        }
        public int GetChildWidth(int i)
        {
            switch (i)
            {
                case 0:
                    return BehindWidth;
                case 1:
                    return content.Width;
                default:
                    return 0;
            }
        }
        public int AboveOffset
        {
            set
            {
                content.SetPadding(value, content.PaddingTop, content.PaddingRight, content.PaddingBottom);
            }
        }

        public bool IsMenuOpen
        {
            get { return curItem == 0 || curItem == 2; }
        }
        public bool IsSlidingEnabled
        {
            get { return enabled; }
            set { enabled = value; }
        }


        protected void InitViewAboveGroup()
        {
            TouchMode = TouchMode.Margin;
            SetWillNotDraw(false);
            DescendantFocusability = DescendantFocusability.AfterDescendants;
            Focusable = true;
            scroller = new Scroller(Context, interpolator);
            var configuration = ViewConfiguration.Get(Context);
            touchSlop = ViewConfigurationCompat.GetScaledPagingTouchSlop(configuration);
            minimumVelocity = configuration.ScaledMinimumFlingVelocity;
            MaximumVelocity = configuration.ScaledMaximumFlingVelocity;

            var density = Context.Resources.DisplayMetrics.Density;
            flingDistance = (int)(MinDistanceForFling * density);

            ResidePageSelected += (sender, args) =>
            {
                if (ViewBehind == null) return;
                switch (args.Position)
                {
                    case 0:
                    case 2:
                        ViewBehind.ChildrenEnabled = true;
                        break;
                    case 1:
                        ViewBehind.ChildrenEnabled = false;
                        break;
                }
            };
        }
        protected void OnPageScrolled(int xpos)
        {
            var widthWithMargin = Width;
            var position = xpos / widthWithMargin;
            var offsetPixels = xpos % widthWithMargin;
            var offset = (float)offsetPixels / widthWithMargin;

            if (null != ResidePageScrolled)
                ResidePageScrolled(this, new ResidePageScrolledEventArgs
                {
                    Position = position,
                    PositionOffset = offset,
                    PositionOffsetPixels = offsetPixels
                });
        }
        protected void SmoothScrollTo(int x, int y, int velocity = 0)
        {
            if (ChildCount == 0)
            {
                ScrollingCacheEnabled = false;
                return;
            }

            var sx = ScrollX;
            var sy = ScrollY;
            var dx = x - sx;
            var dy = y - sy;
            if (dx == 0 && dy == 0)
            {
                CompleteScroll();
                if (IsMenuOpen)
                {
                    if (null != Opened)
                        Opened(this, EventArgs.Empty);
                }
                else
                {
                    if (null != Closed)
                        Closed(this, EventArgs.Empty);
                }
                return;
            }

            ScrollingCacheEnabled = true;
            scrolling = true;

            var width = BehindWidth;
            var halfWidth = width / 2;
            var distanceRatio = Math.Min(1f, 1.0f * Math.Abs(dx) / width);
            var distance = halfWidth + halfWidth * DistanceInfluenceForSnapDuration(distanceRatio);
            int duration;
            velocity = Math.Abs(velocity);
            if (velocity > 0)
                duration = (int)(4 * Math.Round(1000 * Math.Abs(distance / velocity)));
            else
            {
                var pageDelta = (float)Math.Abs(dx) / width;
                duration = (int)((pageDelta + 1) * 100);
            }
            duration = Math.Min(duration, MaxSettleDuration);

            scroller.StartScroll(sx, sy, dx, dy, duration);
            Invalidate();
        }
        protected void SetCurrentItemInternal(int item, bool smoothScroll, bool always, int velocity = 0)
        {
            if (!always && curItem == item)
            {
                ScrollingCacheEnabled = false;
                return;
            }

            item = ViewBehind.GetMenuPage(item);

            var dispatchSelected = curItem != item;
            curItem = item;
            var destX = GetDestScrollX(curItem);
            if (dispatchSelected && ResidePageSelected != null)
            {
                ResidePageSelected(this, new ResidePageSelectedEventArgs { Position = item });
            }
            if (smoothScroll)
            {
                SmoothScrollTo(destX, 0, velocity);
            }
            else
            {
                CompleteScroll();
                ScrollTo(destX, 0);
            }
        }



        private int LeftBound
        {
            get { return ViewBehind.GetAbsLeftBound(content); }
        }
        private int RightBound
        {
            get { return ViewBehind.GetAbsRightBound(content); }
        }
        private bool IsInIgnoredView(MotionEvent ev)
        {
            var rect = new Rect();
            foreach (var v in ignoredViews)
            {
                v.GetHitRect(rect);
                if (rect.Contains((int)ev.GetX(), (int)ev.GetY()))
                    return true;
            }
            return false;
        }

        public View Content
        {
            get { return content; }
            set
            {
                if (content != null)
                    RemoveView(content);
                content = value;
                AddView(content);
            }
        }
        public BehindViewGroup BehindView
        {
            set { ViewBehind = value; }
        }
        public TouchMode TouchMode { get; set; }
        public BehindViewGroup ViewBehind
        {
            get
            {
                return viewBehindGroup;
            }

            set
            {
                viewBehindGroup = value;
            }
        }


        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var width = GetDefaultSize(0, widthMeasureSpec);
            var height = GetDefaultSize(0, heightMeasureSpec);
            SetMeasuredDimension(width, height);

            var contentWidth = GetChildMeasureSpec(widthMeasureSpec, 0, width);
            var contentHeight = GetChildMeasureSpec(heightMeasureSpec, 0, height);
            content.Measure(contentWidth, contentHeight);
        }
        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            if (w == oldw) return;
            CompleteScroll();
            ScrollTo(GetDestScrollX(curItem), ScrollY);
        }
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            var width = r - l;
            var height = b - t;
            content.Layout(0, 0, width, height);
        }
        protected override void DispatchDraw(Canvas canvas)
        {
            base.DispatchDraw(canvas);

            ViewBehind.DrawShadow(content, canvas);
            ViewBehind.DrawFade(content, canvas, PercentOpen);
            ViewBehind.DrawSelector(content, canvas, PercentOpen);
        }

        public override void ComputeScroll()
        {
            if (!scroller.IsFinished)
            {
                if (scroller.ComputeScrollOffset())
                {
                    var oldX = ScrollX;
                    var oldY = ScrollY;
                    var x = scroller.CurrX;
                    var y = scroller.CurrY;

                    if (oldX != x || oldY != y)
                    {
                        ScrollTo(x, y);
                        OnPageScrolled(x);
                    }

                    Invalidate();
                    return;
                }
            }

            CompleteScroll();
        }
        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            if (!enabled)
                return false;

            var action = (int)ev.Action & MotionEventCompat.ActionMask;

            if (action == (int)MotionEventActions.Cancel || action == (int)MotionEventActions.Up ||
                (action != (int)MotionEventActions.Down && isUnableToDrag))
            {
                EndDrag();
                return false;
            }

            switch (action)
            {
                case (int)MotionEventActions.Move:
                    DetermineDrag(ev);
                    break;
                case (int)MotionEventActions.Down:
                    var index = MotionEventCompat.GetActionIndex(ev);
                    ActivePointerId = MotionEventCompat.GetPointerId(ev, index);
                    if (ActivePointerId == InvalidPointer)
                        break;
                    lastMotionX = initialMotionX = MotionEventCompat.GetX(ev, index);
                    lastMotionY = MotionEventCompat.GetY(ev, index);
                    if (ThisTouchAllowed(ev))
                    {
                        isBeingDragged = false;
                        isUnableToDrag = false;
                        if (IsMenuOpen && ViewBehind.MenuTouchInQuickReturn(content, curItem,
                            ev.GetX() + scrollX))
                            quickReturn = true;
                    }
                    else
                        isUnableToDrag = true;
                    break;
                case (int)MotionEventActions.PointerUp:
                    OnSecondaryPointerUp(ev);
                    break;
            }

            if (!isBeingDragged)
            {
                if (VelocityTracker == null)
                    VelocityTracker = VelocityTracker.Obtain();
                VelocityTracker.AddMovement(ev);
            }
            return isBeingDragged || quickReturn;
        }
        public override bool OnTouchEvent(MotionEvent ev)
        {
            if (!enabled)
                return false;

            if (!isBeingDragged && !ThisTouchAllowed(ev))
                return false;

            if (VelocityTracker == null)
                VelocityTracker = VelocityTracker.Obtain();
            VelocityTracker.AddMovement(ev);

            var action = (int)ev.Action & MotionEventCompat.ActionMask;
            switch (action)
            {
                case (int)MotionEventActions.Down:
                    CompleteScroll();

                    var index = MotionEventCompat.GetActionIndex(ev);
                    ActivePointerId = MotionEventCompat.GetPointerId(ev, index);
                    lastMotionX = initialMotionX = ev.GetX();
                    break;
                case (int)MotionEventActions.Move:
                    if (!isBeingDragged)
                    {
                        DetermineDrag(ev);
                        if (isUnableToDrag)
                            return false;
                    }
                    if (isBeingDragged)
                    {
                        var activePointerIndex = GetPointerIndex(ev, ActivePointerId);
                        if (ActivePointerId == InvalidPointer)
                            break;
                        var x = MotionEventCompat.GetX(ev, activePointerIndex);
                        var deltaX = lastMotionX - x;
                        lastMotionX = x;
                        var oldScrollX = ScrollX;
                        var scrollX = oldScrollX + deltaX;
                        var leftBound = LeftBound;
                        var rightBound = RightBound;
                        if (scrollX < leftBound)
                            scrollX = leftBound;
                        else if (scrollX > rightBound)
                            scrollX = rightBound;
                        lastMotionX += scrollX - (int)scrollX;
                        ScrollTo((int)scrollX, ScrollY);
                        OnPageScrolled((int)scrollX);
                    }
                    break;
                case (int)MotionEventActions.Up:
                    if (isBeingDragged)
                    {
                        var velocityTracker = VelocityTracker;
                        velocityTracker.ComputeCurrentVelocity(1000, MaximumVelocity);
                        var initialVelocity =
                            (int)VelocityTrackerCompat.GetXVelocity(velocityTracker, ActivePointerId);
                        var scrollX = ScrollX;
                        var pageOffset = (float)(scrollX - GetDestScrollX(curItem)) / BehindWidth;
                        var activePointerIndex = GetPointerIndex(ev, ActivePointerId);
                        if (ActivePointerId != InvalidPointer)
                        {
                            var x = MotionEventCompat.GetX(ev, activePointerIndex);
                            var totalDelta = (int)(x - initialMotionX);
                            var nextPage = DetermineTargetPage(pageOffset, initialVelocity, totalDelta);
                            SetCurrentItemInternal(nextPage, true, true, initialVelocity);
                        }
                        else
                            SetCurrentItemInternal(curItem, true, true, initialVelocity);
                        ActivePointerId = InvalidPointer;
                        EndDrag();
                    }
                    else if (quickReturn &&
                             ViewBehind.MenuTouchInQuickReturn(content, curItem, ev.GetX() + scrollX))
                    {
                        SetCurrentItem(1);
                        EndDrag();
                    }
                    break;
                case (int)MotionEventActions.Cancel:
                    if (isBeingDragged)
                    {
                        SetCurrentItemInternal(curItem, true, true);
                        ActivePointerId = InvalidPointer;
                        EndDrag();
                    }
                    break;
                case MotionEventCompat.ActionPointerDown:
                    var indexx = MotionEventCompat.GetActionIndex(ev);
                    lastMotionX = MotionEventCompat.GetX(ev, indexx);
                    ActivePointerId = MotionEventCompat.GetPointerId(ev, indexx);
                    break;
                case MotionEventCompat.ActionPointerUp:
                    OnSecondaryPointerUp(ev);
                    var pointerIndex = GetPointerIndex(ev, ActivePointerId);
                    if (ActivePointerId == InvalidPointer)
                        break;
                    lastMotionX = MotionEventCompat.GetX(ev, pointerIndex);
                    break;
            }
            return true;
        }
        public override void ScrollTo(int x, int y)
        {
            base.ScrollTo(x, y);

            scrollX = x;
            ViewBehind.ScrollBehindTo(content, x, y);
#if __ANDROID_11__
            ((ResideMenuLayout)Parent).ManageLayers(PercentOpen);
#endif
        }
        public override bool DispatchKeyEvent(KeyEvent e)
        {
            return base.DispatchKeyEvent(e) || ExecuteKeyEvent(e);
        }

        public float PercentOpen { get { return Math.Abs(scrollX - content.Left) / BehindWidth; } }
        public float ScrollX1
        {
            get
            {
                return scrollX;
            }

            set
            {
                scrollX = value;
            }
        }

        public bool ExecuteKeyEvent(KeyEvent ev)
        {
            var handled = false;
            if (ev.Action == KeyEventActions.Down)
            {
                switch (ev.KeyCode)
                {
                    case Keycode.DpadLeft:
                        handled = ArrowScroll(FocusSearchDirection.Left);
                        break;
                    case Keycode.DpadRight:
                        handled = ArrowScroll(FocusSearchDirection.Right);
                        break;
                    case Keycode.Tab:
                        if ((int)Build.VERSION.SdkInt >= 11)
                        {
                            if (KeyEventCompat.HasNoModifiers(ev))
                                handled = ArrowScroll(FocusSearchDirection.Forward);
#if __ANDROID_11__
                            else if (ev.IsMetaPressed)
                                handled = ArrowScroll(FocusSearchDirection.Backward);
#endif
                        }
                        break;
                }
            }
            return handled;
        }
        public bool ArrowScroll(FocusSearchDirection direction)
        {
            var currentFocused = FindFocus();

            var handled = false;

            var nextFocused = FocusFinder.Instance.FindNextFocus(this, currentFocused == this ? null : currentFocused, direction);
            if (nextFocused != null && nextFocused != currentFocused)
            {
                if (direction == FocusSearchDirection.Left)
                    handled = nextFocused.RequestFocus();
                else if (direction == FocusSearchDirection.Right)
                {
                    if (currentFocused != null && nextFocused.Left <= currentFocused.Left)
                        handled = PageRight();
                    else
                        handled = nextFocused.RequestFocus();
                }
            }
            else if (direction == FocusSearchDirection.Left || direction == FocusSearchDirection.Backward)
                handled = PageLeft();
            else if (direction == FocusSearchDirection.Right || direction == FocusSearchDirection.Forward)
                handled = PageRight();

            if (handled)
                PlaySoundEffect(SoundEffectConstants.GetContantForFocusDirection(direction));

            return handled;
        }
        protected bool CanScroll(View v, bool checkV, int dx, int x, int y)
        {
            var viewGroup = v as ViewGroup;
            if (viewGroup != null)
            {
                var scrollX = v.ScrollX;
                var scrollY = v.ScrollY;
                var count = viewGroup.ChildCount;

                for (var i = count - 1; i >= 0; i--)
                {
                    var child = viewGroup.GetChildAt(i);
                    if (x + scrollX >= child.Left && x + scrollX < child.Right &&
                        y + scrollY >= child.Top && y + scrollY < child.Bottom &&
                        CanScroll(child, true, dx, x + scrollX - child.Left, y + scrollY - child.Top))
                        return true;
                }
            }
            return checkV && ViewCompat.CanScrollHorizontally(v, -dx);
        }

        protected bool PageLeft()
        {
            if (curItem > 0)
            {
                SetCurrentItem(curItem - 1, true);
                return true;
            }
            return false;
        }
        protected bool PageRight()
        {
            if (curItem < 1)
            {
                SetCurrentItem(curItem + 1, true);
                return true;
            }
            return false;
        }


        private void CompleteScroll()
        {
            var needPopulate = scrolling;
            if (needPopulate)
            {
                ScrollingCacheEnabled = false;
                scroller.AbortAnimation();
                var oldX = ScrollX;
                var oldY = ScrollY;
                var x = scroller.CurrX;
                var y = scroller.CurrY;
                if (oldX != x || oldY != y)
                    ScrollTo(x, y);

                if (IsMenuOpen)
                {
                    if (null != Opened)
                        Opened(this, EventArgs.Empty);
                }
                else
                {
                    if (null != Closed)
                        Closed(this, EventArgs.Empty);
                }
            }
            scrolling = false;
        }
        private bool ThisTouchAllowed(MotionEvent ev)
        {
            var x = (int)(ev.GetX() + scrollX);
            if (IsMenuOpen)
            {
                return ViewBehind.MenuOpenTouchAllowed(content, curItem, x);
            }
            switch (TouchMode)
            {
                case TouchMode.Fullscreen:
                    return !IsInIgnoredView(ev);
                case TouchMode.None:
                    return false;
                case TouchMode.Margin:
                    return ViewBehind.MarginTouchAllowed(content, x);
            }
            return false;
        }
        private bool ThisSlideAllowed(float dx)
        {
            var allowed = IsMenuOpen ? ViewBehind.MenuOpenSlideAllowed(dx)
                               : ViewBehind.MenuClosedSlideAllowed(dx);
            return allowed;
        }

        private int GetPointerIndex(MotionEvent ev, int id)
        {
            var activePointerIndex = MotionEventCompat.FindPointerIndex(ev, id);
            if (activePointerIndex == -1)
                ActivePointerId = InvalidPointer;
            return activePointerIndex;
        }
        private int DetermineTargetPage(float pageOffset, int velocity, int deltaX)
        {
            var targetPage = curItem;
            if (Math.Abs(deltaX) > flingDistance && Math.Abs(velocity) > minimumVelocity)
            {
                if (velocity > 0 && deltaX > 0)
                    targetPage -= 1;
                else if (velocity < 0 && deltaX < 0)
                    targetPage += 1;
            }
            else
                targetPage = (int)Math.Round(curItem + pageOffset);
            return targetPage;
        }

        private void DetermineDrag(MotionEvent ev)
        {
            var activePointerId = ActivePointerId;
            var pointerIndex = GetPointerIndex(ev, activePointerId);
            if (activePointerId == InvalidPointer || pointerIndex == InvalidPointer)
                return;
            var x = MotionEventCompat.GetX(ev, pointerIndex);
            var dx = x - lastMotionX;
            var xDiff = Math.Abs(dx);
            var y = MotionEventCompat.GetY(ev, pointerIndex);
            var dy = y - lastMotionY;
            var yDiff = Math.Abs(dy);
            if (xDiff > (IsMenuOpen ? touchSlop / 2 : touchSlop) && xDiff > yDiff && ThisSlideAllowed(dx))
            {
                StartDrag();
                lastMotionX = x;
                lastMotionY = y;
                scrollingCacheEnabled = true;
            }
            else if (xDiff > touchSlop)
                isUnableToDrag = true;
        }
        private void OnSecondaryPointerUp(MotionEvent ev)
        {
            var pointerIndex = MotionEventCompat.GetActionIndex(ev);
            var pointerId = MotionEventCompat.GetPointerId(ev, pointerIndex);
            if (pointerId == ActivePointerId)
            {
                var newPointerIndex = pointerIndex == 0 ? 1 : 0;
                lastMotionX = MotionEventCompat.GetX(ev, newPointerIndex);
                ActivePointerId = MotionEventCompat.GetPointerId(ev, newPointerIndex);
                if (VelocityTracker != null)
                    VelocityTracker.Clear();
            }
        }
        private void StartDrag()
        {
            isBeingDragged = true;
            quickReturn = false;
        }
        private void EndDrag()
        {
            quickReturn = false;
            isBeingDragged = false;
            isUnableToDrag = false;
            ActivePointerId = InvalidPointer;

            if (VelocityTracker == null) return;
            VelocityTracker.Recycle();
            VelocityTracker = null;
        }
        private bool ScrollingCacheEnabled
        {
            set
            {
                if (scrollingCacheEnabled != value)
                {
                    scrollingCacheEnabled = value;
                    if (UseCache)
                    {
                        var size = ChildCount;
                        for (var i = 0; i < size; ++i)
                        {
                            var child = GetChildAt(i);
                            if (child.Visibility != ViewStates.Gone)
                                child.DrawingCacheEnabled = value;
                        }
                    }
                }
            }
        }

    }
}