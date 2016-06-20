using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using MvvmCross.Droid.Views;
using Mobet.Metting.Droid.Fragments;
using Mobet.Metting.Models;
using MvvmCross.Droid.Support.V4;
using Mobet.Droid.Components.AmazingRefresharp.Views;

namespace Mobet.Metting.Droid
{
    [Activity(Label = "遇见", Theme = "@style/Theme.DeviceDefaultLightNoActionBar")]
    public class MainActivity : MvxFragmentActivity<MainModel> //global::Android.Support.V4.App.FragmentActivity
    {

        public static readonly List<String> FLAVORS = new List<string>() {
            "Donut", "Eclair", "Froyo", "Gingerbread", "Honeycomb", "Ice Cream Sandwich", "Jelly Bean"
        };

        protected override void OnCreate(Bundle bundle)
        {
            SetTheme(Android.Resource.Style.ThemeDeviceDefaultLightNoActionBar);
            Window.AddFlags(WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(WindowManagerFlags.TranslucentNavigation);
            base.OnCreate(bundle);


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            this.SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.fragment_container, new SampleListFragment())
                .Commit();
        }
    }
}

