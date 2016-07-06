using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Mobet.Droid.Components.ResideMenu.CanvasTransformer;
using Mobet.Droid.Components.ResideMenu.Enums;
using Mobet.Droid.Components.ResideMenu.ViewGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobet.Droid.Components.ResideMenu
{
    public class ResideMenuLayout : RelativeLayout
    {
        private new const string Tag = "ResideMenu";
        private bool actionbarOverlay;

        private readonly AboveViewGroup viewAbove;
        private readonly BehindViewGroup viewBehind;

        public event EventHandler Open;
        public event EventHandler Close;
        public event EventHandler Opened;
        public event EventHandler Closed;

        public ResideMenuLayout(Context context)
            : this(context, null)
        {
        }

        public ResideMenuLayout(Context context, IAttributeSet attrs)
            : this(context, attrs, 0)
        {
        }

        public ResideMenuLayout(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            var behindParams = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            viewBehind = new BehindViewGroup(context);
            AddView(viewBehind, behindParams);

            var aboveParams = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            this.viewAbove = new AboveViewGroup(context);
            base.AddView(this.viewAbove, aboveParams);

            this.viewAbove.BehindView = viewBehind;
            viewBehind.AboveView = this.viewAbove;

            this.viewAbove.ResidePageSelected += (sender, args) =>
            {
                if (args.Position == 0 && null != Open) //position open
                    Open(this, EventArgs.Empty);
                else if (args.Position == 2 && null != Close) //position close
                    Close(this, EventArgs.Empty);
            };

            this.viewAbove.Opened += (sender, args) => { if (null != Opened) Opened(sender, args); };
            this.viewAbove.Closed += (sender, args) => { if (null != Closed) Closed(sender, args); };

            var a = context.ObtainStyledAttributes(attrs, Resource.Styleable.reside_menu);
            var mode = a.GetInt(Resource.Styleable.reside_menu_mode, (int)MenuMode.Left);
            Mode = (MenuMode)mode;

            var viewAbove = a.GetResourceId(Resource.Styleable.reside_menu_view_above, -1);
            if (viewAbove != -1)
                SetContent(viewAbove);
            else
                SetContent(new FrameLayout(context));

            TouchModeAbove = (TouchMode)a.GetInt(Resource.Styleable.reside_menu_touch_mode_above, (int)TouchMode.Margin);
            TouchModeBehind = (TouchMode)a.GetInt(Resource.Styleable.reside_menu_touch_mode_behind, (int)TouchMode.Margin);

            var offsetBehind = (int)a.GetDimension(Resource.Styleable.reside_menu_behind_offset, -1);
            var widthBehind = (int)a.GetDimension(Resource.Styleable.reside_menu_behind_width, -1);
            if (offsetBehind != -1 && widthBehind != -1)
                throw new ArgumentException("Cannot set both behindOffset and behindWidth for reside_menu, check your XML");
            if (offsetBehind != -1)
                BehindOffset = offsetBehind;
            else if (widthBehind != -1)
                BehindWidth = widthBehind;
            else
                BehindOffset = 0;

            var shadowRes = a.GetResourceId(Resource.Styleable.reside_menu_shadow_drawable, -1);
            if (shadowRes != -1)
                ShadowDrawableRes = shadowRes;

            BehindScrollScale = a.GetFloat(Resource.Styleable.reside_menu_behind_scroll_scale, 0.33f);
            ShadowWidth = ((int)a.GetDimension(Resource.Styleable.reside_menu_shadow_width, 0));
            FadeEnabled = a.GetBoolean(Resource.Styleable.reside_menu_fade_enabled, true);
            FadeDegree = a.GetFloat(Resource.Styleable.reside_menu_fade_degree, 0.33f);
            SelectorEnabled = a.GetBoolean(Resource.Styleable.reside_menu_selector_enabled, false);
            var selectorRes = a.GetResourceId(Resource.Styleable.reside_menu_selector_drawable, -1);
            if (selectorRes != -1)
                SelectorDrawable = selectorRes;

            a.Recycle();
        }


        public void AttachToActivity(Activity activity, ResideStyle resideStyle)
        {
            AttachToActivity(activity, resideStyle, true);
        }

        public void AttachToActivity(Activity activity, ResideStyle resideStyle, bool actionbarOverlay)
        {
            if (Parent != null)
                throw new ArgumentException("This reside_menu appears to already be attached");

            // get the window background
            var a = activity.Theme.ObtainStyledAttributes(new[] { Android.Resource.Attribute.WindowBackground });
            var background = a.GetResourceId(0, 0);
            a.Recycle();

            switch (resideStyle)
            {
                case ResideStyle.Window:
                    this.actionbarOverlay = true;
                    var decor = (ViewGroup)activity.Window.DecorView;
                    var decorChild = (ViewGroup)decor.GetChildAt(0);
                    // save ActionBar themes that have transparent assets
                    decorChild.SetBackgroundResource(background);
                    decor.RemoveView(decorChild);
                    decor.AddView(this);
                    SetContent(decorChild);
                    break;
                case ResideStyle.Content:
                    this.actionbarOverlay = actionbarOverlay;
                    // take the above view out of
                    var contentParent = (ViewGroup)activity.FindViewById(Android.Resource.Id.Content);
                    var content = contentParent.GetChildAt(0);
                    contentParent.RemoveView(content);
                    contentParent.AddView(this);
                    SetContent(content);
                    // save people from having transparent backgrounds
                    if (content.Background == null)
                        content.SetBackgroundResource(background);
                    break;
            }
        }


        public void SetContent(int res)
        {
            SetContent(LayoutInflater.From(Context).Inflate(res, null));
        }

        public void SetContent(View view)
        {
            viewAbove.Content = view;
            ShowContent();
        }

        public void SetMenu(int res)
        {
            SetMenu(LayoutInflater.From(Context).Inflate(res, null));
        }

        public void SetMenu(View v)
        {
            viewBehind.Content = v;
        }


        public View GetMenu()
        {
            return viewBehind.Content;
        }
        public View GetContent()
        {
            return viewAbove.Content;
        }


        public void SetSecondaryMenu(int res)
        {
            SetSecondaryMenu(LayoutInflater.From(Context).Inflate(res, null));
        }

        public void SetSecondaryMenu(View v)
        {
            viewBehind.SecondaryContent = v;
        }

        public View GetSecondaryMenu()
        {
            return viewBehind.SecondaryContent;
        }

        public bool IsSlidingEnabled
        {
            get { return viewAbove.IsSlidingEnabled; }
            set { viewAbove.IsSlidingEnabled = value; }
        }

        public MenuMode Mode
        {
            get { return viewBehind.Mode; }
            set
            {
                if (value != MenuMode.Left && value != MenuMode.Right && value != MenuMode.LeftRight)
                {
                    throw new ArgumentException("reside_menu mode must be LEFT, RIGHT, or LEFT_RIGHT", "value");
                }
                viewBehind.Mode = value;
            }
        }

        public bool Static
        {
            set
            {
                if (value)
                {
                    IsSlidingEnabled = false;
                    viewAbove.BehindView = null;
                    viewAbove.SetCurrentItem(1);
                }
                else
                {
                    viewAbove.SetCurrentItem(1);
                    viewAbove.BehindView = viewBehind;
                    IsSlidingEnabled = true;
                }
            }
        }

        public void ShowMenu()
        {
            ShowMenu(true);
        }
        public void ShowMenu(bool animate)
        {
            viewAbove.SetCurrentItem(0, animate);
        }
        public void ShowSecondaryMenu()
        {
            ShowSecondaryMenu(true);
        }
        public void ShowSecondaryMenu(bool animate)
        {
            viewAbove.SetCurrentItem(2, animate);
        }
        public void ShowContent(bool animate = true)
        {
            viewAbove.SetCurrentItem(1, animate);
        }

        public void Toggle()
        {
            Toggle(true);
        }
        public void Toggle(bool animate)
        {
            if (IsMenuShowing)
            {
                ShowContent(animate);
            }
            else
            {
                ShowMenu(animate);
            }
        }

        public bool IsMenuShowing
        {
            get { return viewAbove.GetCurrentItem() == 0 || viewAbove.GetCurrentItem() == 2; }
        }
        public bool IsSecondaryMenuShowing
        {
            get { return viewAbove.GetCurrentItem() == 2; }
        }

        public int BehindOffset
        {
            get { return viewBehind.WidthOffset; }
            set
            {
                viewBehind.WidthOffset = value;
            }
        }
        public int BehindOffsetRes
        {
            set
            {
                var i = (int)Context.Resources.GetDimension(value);
                BehindOffset = i;
            }
        }

        public int AboveOffset
        {
            set { viewAbove.AboveOffset = value; }
        }
        public int AboveOffsetRes
        {
            set
            {
                var i = (int)Context.Resources.GetDimension(value);
                AboveOffset = i;
            }
        }

        public int BehindWidth
        {
            set
            {
                var windowService = Context.GetSystemService(Context.WindowService);
                var windowManager = windowService.JavaCast<IWindowManager>();
                var width = windowManager.DefaultDisplay.Width;
                BehindOffset = width - value;
            }
        }
        public int BehindWidthRes
        {
            set
            {
                var i = (int)Context.Resources.GetDimension(value);
                BehindWidth = i;
            }
        }
        public float BehindScrollScale
        {
            get { return viewBehind.ScrollScale; }
            set
            {
                if (value < 0f && value > 1f)
                    throw new ArgumentOutOfRangeException("value", "ScrollScale must be between 0f and 1f");
                viewBehind.ScrollScale = value;
            }
        }

        public int TouchmodeMarginThreshold
        {
            get { return viewBehind.MarginThreshold; }
            set { viewBehind.MarginThreshold = value; }
        }

        public TouchMode TouchModeAbove
        {
            get { return viewAbove.TouchMode; }
            set
            {
                if (value != TouchMode.Fullscreen && value != TouchMode.Margin
                    && value != TouchMode.None)
                {
                    throw new ArgumentException("TouchMode must be set to either" +
                            "TOUCHMODE_FULLSCREEN or TOUCHMODE_MARGIN or TOUCHMODE_NONE.", "value");
                }
                viewAbove.TouchMode = value;
            }
        }
        public TouchMode TouchModeBehind
        {
            set
            {
                if (value != TouchMode.Fullscreen && value != TouchMode.Margin
                    && value != TouchMode.None)
                {
                    throw new ArgumentException("TouchMode must be set to either" +
                            "TOUCHMODE_FULLSCREEN or TOUCHMODE_MARGIN or TOUCHMODE_NONE.", "value");
                }
                viewBehind.TouchMode = value;
            }
        }

        public int ShadowDrawableRes
        {
            set { viewBehind.ShadowDrawable = Context.Resources.GetDrawable(value); }
        }
        public Drawable ShadowDrawable
        {
            set { viewBehind.ShadowDrawable = value; }
        }

        public int SecondaryShadowDrawableRes
        {
            set { viewBehind.SecondaryShadowDrawable = Context.Resources.GetDrawable(value); }
        }
        public Drawable SecondaryShadowDrawable
        {
            set { viewBehind.SecondaryShadowDrawable = value; }
        }

        public int ShadowWidthRes
        {
            set { ShadowWidth = (int)Context.Resources.GetDimension(value); }
        }
        public int ShadowWidth
        {
            set { viewBehind.ShadowWidth = value; }
        }

        public bool FadeEnabled
        {
            set { viewBehind.FadeEnabled = value; }
        }
        public float FadeDegree
        {
            set { viewBehind.FadeDegree = value; }
        }

        public bool SelectorEnabled
        {
            set { viewBehind.SelectorEnabled = value; }
        }
        public View SelectedView
        {
            set { viewBehind.SelectedView = value; }
        }
        public int SelectorDrawable
        {
            set { SelectorBitmap = BitmapFactory.DecodeResource(Resources, value); }
        }
        public Bitmap SelectorBitmap
        {
            set { viewBehind.SelectorBitmap = value; }
        }

        public void AddIgnoredView(View v)
        {
            viewAbove.AddIgnoredView(v);
        }
        public void RemoveIgnoredView(View v)
        {
            viewAbove.RemoveIgnoredView(v);
        }
        public void ClearIgnoredViews()
        {
            viewAbove.ClearIgnoredViews();
        }

        public ICanvasTransformer BehindCanvasTransformer
        {
            set { viewBehind.CanvasTransformer = value; }
        }
        public class SavedState : BaseSavedState
        {
            public int Item { get; private set; }

            public SavedState(IParcelable superState)
                : base(superState)
            {

            }

            public SavedState(Parcel parcel)
                : base(parcel)
            {
                Item = parcel.ReadInt();
            }

            public override void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
            {
                base.WriteToParcel(dest, flags);
                dest.WriteInt(Item);
            }

            [ExportField("CREATOR")]
            static SavedStateCreator InitializeCreator()
            {
                return new SavedStateCreator();
            }

            class SavedStateCreator : Java.Lang.Object, IParcelableCreator
            {
                public Java.Lang.Object CreateFromParcel(Parcel source)
                {
                    return new SavedState(source);
                }

                public Java.Lang.Object[] NewArray(int size)
                {
                    return new SavedState[size];
                }
            }
        }

        protected override void OnRestoreInstanceState(IParcelable state)
        {
            try
            {
                Bundle bundle = state as Bundle;
                if (bundle != null)
                {
                    IParcelable superState = (IParcelable)bundle.GetParcelable("base");
                    if (superState != null)
                        base.OnRestoreInstanceState(superState);
                    viewAbove.SetCurrentItem(bundle.GetInt("currentPosition", 0));
                }
            }
            catch
            {
                base.OnRestoreInstanceState(state);
                // Ignore, this needs to support IParcelable...
            }
        }

        protected override IParcelable OnSaveInstanceState()
        {
            var superState = base.OnSaveInstanceState();
            Bundle state = new Bundle();
            state.PutParcelable("base", superState);
            state.PutInt("currentPosition", viewAbove.GetCurrentItem());

            return state;
        }

        protected override bool FitSystemWindows(Rect insets)
        {
            if (!actionbarOverlay)
            {
                Log.Verbose(Tag, "setting padding");
                SetPadding(insets.Left, insets.Top, insets.Right, insets.Bottom);
            }
            return true;
        }

#if __ANDROID_11__
        public void ManageLayers(float percentOpen)
        {
            if ((int)Build.VERSION.SdkInt < 11) return;

            var layer = percentOpen > 0.0f && percentOpen < 1.0f;
            var layerType = layer ? LayerType.Hardware : LayerType.None;

            if (layerType != GetContent().LayerType)
            {
                Handler.Post(() =>
                {
                    Log.Verbose(Tag, "changing layerType, hardware? " + (layerType == LayerType.Hardware));
                    GetContent().SetLayerType(layerType, null);
                    GetMenu().SetLayerType(layerType, null);
                    if (GetSecondaryMenu() != null)
                        GetSecondaryMenu().SetLayerType(layerType, null);
                });
            }
        }
#endif
    }
}
