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

using SupportFragment = Android.Support.V4.App.Fragment;

using Mobet.Droid.Components.AmazingRefresharp.Views;
using Mobet.Droid.Components.AmazingRefresharp.SwipeMenuList;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Util;
using Mobet.Metting.Droid.Adapters;

namespace Mobet.Metting.Droid.Fragments
{
    public class CallFragment : SupportFragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup root, Bundle data)
        {
            return inflater.Inflate(Resource.Layout.main_frame_call, null, false);
        }
    }
}