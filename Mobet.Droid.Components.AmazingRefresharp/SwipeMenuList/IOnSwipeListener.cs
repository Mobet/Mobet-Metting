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

namespace Mobet.Droid.Components.AmazingRefresharp.SwipeMenuList
{
    public interface IOnSwipeListener
    {
        void OnSwipeStart(int position);
        void OnSwipeEnd(int position);
    }
}