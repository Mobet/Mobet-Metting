using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Views.Animations;

using Mobet.Droid.Components.AmazingRefresharp.Views;
using Mobet.Droid.Components.AmazingRefresharp.Delegates;
using Mobet.Droid.Components.AmazingRefresharp.SwipeMenuList;

namespace Mobet.Droid.Components.AmazingRefresharp.Widget
{
    public class ListView : global::Android.Widget.ListView, IAmazingRefreshsharpWrappedView
    {
        private const int TOUCH_STATE_NONE = 0;
        private const int TOUCH_STATE_X = 1;
        private const int TOUCH_STATE_Y = 2;

        private int MAX_Y = 5;
        private int MAX_X = 3;
        private float mDownX;
        private float mDownY;
        private int mTouchState;
        private int mTouchPosition;

        private SwipeMenuLayout mTouchView;
        private IOnSwipeListener mOnSwipeListener;

        private ISwipeMenuCreator mMenuCreator;
        private IOnMenuItemClickListener mOnMenuItemClickListner;

        public IInterpolator CloseInterpolator { get; set; }
        public IInterpolator OpenInterpolator { get; set; }

        ListViewDelegate ptr_delegate;


        public ListView(IntPtr javaReference, global::Android.Runtime.JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public ListView(Context context) : this(context, null, 0) { }

        public ListView(Context context, IAttributeSet attrs) : this(context, attrs, 0) { }

        public ListView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            ptr_delegate = new ListViewDelegate(this);
            Init();
        }




        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return base.OnInterceptTouchEvent(ev);
        }


        public override bool OnTouchEvent(MotionEvent e)
        {
            if (!(e.Action != MotionEventActions.Down && mTouchView == null))
            {
                switch (e.Action)
                {
                    case MotionEventActions.Down:
                        {
                            int oldPos = mTouchPosition;
                            mDownX = e.RawX;
                            mDownY = e.RawY;
                            mTouchState = TOUCH_STATE_NONE;

                            mTouchPosition = PointToPosition((int)e.GetX(), (int)e.GetY());

                            if (mTouchPosition == oldPos && mTouchView != null
                                  && mTouchView.IsOpen())
                            {
                                mTouchState = TOUCH_STATE_X;
                                mTouchView.OnSwipe(e);
                                return true;
                            }

                            View view = GetChildAt(mTouchPosition - FirstVisiblePosition);

                            if (mTouchView != null && mTouchView.IsOpen())
                            {
                                mTouchView.SmoothCloseMenu();
                                mTouchView = null;

                                MotionEvent cancelEvent = MotionEvent.Obtain(e);
                                cancelEvent.Action = MotionEventActions.Cancel;
                                OnTouchEvent(cancelEvent);
                                return true;
                            }
                            if (view is SwipeMenuLayout)
                            {
                                mTouchView = (SwipeMenuLayout)view;
                            }
                            if (mTouchView != null)
                            {
                                mTouchView.OnSwipe(e);
                            }
                        }
                        break;
                    case MotionEventActions.Move:
                        {
                            float dy = Math.Abs(e.RawY - mDownY);
                            float dx = Math.Abs(e.RawX - mDownX);
                            if (mTouchState == TOUCH_STATE_X)
                            {
                                if (mTouchView != null)
                                {
                                    mTouchView.OnSwipe(e);
                                }
                                Selector.SetState(new int[] { 0 });
                                e.Action = MotionEventActions.Cancel;
                                base.OnTouchEvent(e);
                                return true;
                            }
                            else if (mTouchState == TOUCH_STATE_NONE)
                            {
                                if (Math.Abs(dy) > MAX_Y)
                                {
                                    mTouchState = TOUCH_STATE_Y;
                                }
                                else if (dx > MAX_X)
                                {
                                    mTouchState = TOUCH_STATE_X;
                                    if (mOnSwipeListener != null)
                                    {
                                        mOnSwipeListener.OnSwipeStart(mTouchPosition);
                                    }
                                }
                            }
                        }
                        break;
                    case MotionEventActions.Up:
                        {
                            if (mTouchState == TOUCH_STATE_X)
                            {
                                mTouchView.OnSwipe(e);
                                if (!mTouchView.IsOpen())
                                {
                                    mTouchPosition = -1;
                                    mTouchView = null;
                                }
                                if (mOnSwipeListener != null)
                                {
                                    mOnSwipeListener.OnSwipeEnd(mTouchPosition);
                                }
                                e.Action = MotionEventActions.Cancel;
                                base.OnTouchEvent(e);
                                return true;
                            }
                        }
                        break;
                }
            }
            if (mTouchState != TOUCH_STATE_X)
            {
                return (Parent as ViewWrapper).OnTouchEvent(e) || IgnoreTouchEvents || base.OnTouchEvent(e);
            }
            else
            {
                return base.OnTouchEvent(e);
            }
        }



        public void ForceHandleTouchEvent(MotionEvent e)
        {
            base.OnTouchEvent(e);
        }

        public event EventHandler RefreshActivated
        {
            add { ptr_delegate.RefreshActivated += value; }
            remove { ptr_delegate.RefreshActivated -= value; }
        }

        public event EventHandler RefreshCompleted
        {
            add { ptr_delegate.RefreshCompleted += value; }
            remove { ptr_delegate.RefreshCompleted -= value; }
        }

        public void SetTopMargin(int margin)
        {
            ptr_delegate.SetTopMargin(margin);
        }

        public AmazingRefreshsharpRefreshState RefreshState
        {
            get
            {
                return (Parent as ViewWrapper).State;
            }
        }

        public bool AmazingRefreshEnabled
        {
            get
            {
                return (Parent as ViewWrapper).IsPullEnabled;
            }
            set
            {
                (Parent as ViewWrapper).IsPullEnabled = value;
            }
        }

        public bool IsAtTop
        {
            get
            {
                return ptr_delegate.IsAtTop;
            }
        }

        public bool IgnoreTouchEvents
        {
            get
            {
                return ptr_delegate.IgnoreTouchEvents;
            }
            set
            {
                ptr_delegate.IgnoreTouchEvents = value;
            }
        }

        public void OnRefreshCompleted()
        {
            ptr_delegate.OnRefreshCompleted();
        }

        public void OnRefreshActivated()
        {
            ptr_delegate.OnRefreshActivated();
        }


        private void Init()
        {
            MAX_X = Dp2Px(MAX_X);
            MAX_Y = Dp2Px(MAX_Y);
            mTouchState = TOUCH_STATE_NONE;
        }

        public override IListAdapter Adapter
        {
            get
            {
                return base.Adapter;
            }
            set
            {
                var newAdapter = new SwipeMenuAdapter(Context, value);
                newAdapter.SetSwipeMenuCreator((x) =>
                {
                    if (mMenuCreator != null)
                    {
                        mMenuCreator.Create(x);
                    }
                });
                newAdapter.SetOnMenuItemClickDelegate((x, y, z) =>
                {
                    bool flag = false;
                    if (mOnMenuItemClickListner != null)
                    {
                        flag = mOnMenuItemClickListner.OnMenuItemClick(
                            x.Position, y, z);
                    }
                    if (mTouchView != null && !flag)
                    {
                        mTouchView.SmoothCloseMenu();
                    }
                });
                base.Adapter = newAdapter;
            }
        }

        private int Dp2Px(int dp)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp,
                    Context.Resources.DisplayMetrics);
        }

        public void SmoothOpenMenu(int position)
        {
            if (position >= FirstVisiblePosition && position <= LastVisiblePosition)
            {
                View view = GetChildAt(position - FirstVisiblePosition);
                if (view is SwipeMenuLayout)
                {
                    mTouchPosition = position;
                    if (mTouchView != null && mTouchView.IsOpen())
                    {
                        mTouchView.SmoothCloseMenu();
                    }
                    mTouchView = (SwipeMenuLayout)view;
                    mTouchView.SmoothOpenMenu();
                }
            }
        }

        public void SetMenuCreator(ISwipeMenuCreator menuCreator)
        {
            this.mMenuCreator = menuCreator;
        }

        public void SetOnMenuItemClickListener(IOnMenuItemClickListener onMenuItemClickListener)
        {
            this.mOnMenuItemClickListner = onMenuItemClickListener;
        }

        public void SetOnSwipeListener(IOnSwipeListener onSwipeListener)
        {
            this.mOnSwipeListener = onSwipeListener;
        }

    }
}

