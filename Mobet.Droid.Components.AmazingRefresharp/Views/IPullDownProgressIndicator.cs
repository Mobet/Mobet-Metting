using System;

namespace Mobet.Droid.Components.AmazingRefresharp.Views
{
    public interface IPullDownProgressIndicator
    {
        void SetProgress(float progress);
        void SetRefreshState(AmazingRefreshsharpRefreshState state);
    }
}

