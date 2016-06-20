
using System;

namespace Mobet.Droid.Components.AmazingRefresharp.Views
{
    public interface IAmazingRefreshsharpView
    {
        /// <summary>
        /// Enables/Disables pull to refresh for this view.
        /// </summary>
        /// <value><c>true</c> if pull to refresh enabled; otherwise, <c>false</c>.</value>
        bool AmazingRefreshEnabled { get; set; }

        /// <summary>
        /// Returns the current state of the pull to refresh.
        /// </summary>
        /// <value>The refresh state.</value>
        AmazingRefreshsharpRefreshState RefreshState { get; }

        /// <summary>
        /// Occurs when refresh activated. Hook into this event so you can trigger a refresh
        /// of the content for the view.
        /// </summary>
        event EventHandler RefreshActivated;

        /// <summary>
        /// Raises the refresh completed event. Call this when your refresh is complete
        /// and you wish to hide the pull to refresh header.
        /// </summary>
        void OnRefreshCompleted();
    }
}

