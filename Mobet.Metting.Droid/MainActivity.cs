using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using MvvmCross.Droid.Views;
using Mobet.Metting.Models;

namespace Mobet.Metting.Droid
{
    [Activity(Label = "遇见", Theme = "@style/Theme.DeviceDefaultLightNoActionBar")]
    public class MainActivity : MvxActivity<MainModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            SetTheme(Android.Resource.Style.ThemeDeviceDefaultLightNoActionBar);
            Window.AddFlags(WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(WindowManagerFlags.TranslucentNavigation);
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Setting);
        }
    }
}

