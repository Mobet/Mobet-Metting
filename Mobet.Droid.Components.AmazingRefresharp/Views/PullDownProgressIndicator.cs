using System;
using Android.Widget;
using Android.Content;
using Android.Util;

namespace Mobet.Droid.Components.AmazingRefresharp.Views
{
    public class PullDownProgressIndicator : ViewSwitcher, IPullDownProgressIndicator
    {
        private IAmazingRefreshsharpPullDownIcon icon;


        public PullDownProgressIndicator(Context context) : base(context)
        {
        }

        public PullDownProgressIndicator(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }



        public void SetProgress(float progress)
        {
            icon = icon ?? (IAmazingRefreshsharpPullDownIcon)GetChildAt(0);
            if (icon != null) {
                icon.SetProgress(progress);
            }
        }

        public void SetRefreshState(AmazingRefreshsharpRefreshState state)
        {
            DisplayedChild = state == AmazingRefreshsharpRefreshState.Refreshing ? 1 : 0;
        }

    }
}

