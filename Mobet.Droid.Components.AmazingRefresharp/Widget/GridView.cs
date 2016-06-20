using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Views;

using Mobet.Droid.Components.AmazingRefresharp.Views;
using Mobet.Droid.Components.AmazingRefresharp.Delegates;

namespace Mobet.Droid.Components.AmazingRefresharp.Widget
{
    public class GridView : global::Android.Widget.GridView, IAmazingRefreshsharpWrappedView
    {
        private ListViewDelegate ptr_delegate;

        
        public GridView(Context context) : this(context, null, 0)
        {
        }
        
        public GridView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }
        
        public GridView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            ptr_delegate = new ListViewDelegate(this);
        }
        


        public override bool OnTouchEvent(MotionEvent e)
        {
            if (Parent is ViewWrapper) {
                return (Parent as ViewWrapper).OnTouchEvent(e) || IgnoreTouchEvents || base.OnTouchEvent(e);
            } else {
                return base.OnTouchEvent(e);
            }
        }



        public void ForceHandleTouchEvent(MotionEvent e)
        {
            base.OnTouchEvent(e);
        }

        public event EventHandler RefreshActivated {
            add { ptr_delegate.RefreshActivated += value; }
            remove { ptr_delegate.RefreshActivated -= value; }
        }

        public event EventHandler RefreshCompleted {
            add { ptr_delegate.RefreshCompleted += value; }
            remove { ptr_delegate.RefreshCompleted -= value; }
        }

        public void SetTopMargin(int margin)
        {
            ptr_delegate.SetTopMargin(margin);
        }

        public AmazingRefreshsharpRefreshState RefreshState {
            get {
                return (Parent as ViewWrapper).State;
            }
        }

        public bool AmazingRefreshEnabled {
            get {
                return (Parent as ViewWrapper).IsPullEnabled;
            }
            set {
                (Parent as ViewWrapper).IsPullEnabled = value;
            }
        }

        public bool IsAtTop {
            get {
                return ptr_delegate.IsAtTop;
            }
        }

        public bool IgnoreTouchEvents {
            get {
                return ptr_delegate.IgnoreTouchEvents;
            }
            set {
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

    }
}

