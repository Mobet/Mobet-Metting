using System;
using Android.Views;

namespace Mobet.Droid.Components.AmazingRefresharp.Views
{
    public interface IAmazingRefreshsharpWrappedView : IAmazingRefreshsharpView
    {
        event EventHandler RefreshCompleted;

        void OnRefreshActivated();
        void SetTopMargin(int margin);
        void ForceHandleTouchEvent(MotionEvent e);

        bool IsAtTop { get; }
        bool IgnoreTouchEvents { get; set; }
    }
}

