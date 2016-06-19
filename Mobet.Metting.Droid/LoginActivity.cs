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

using MvvmCross.Droid.FullFragging.Views;
using Mobet.Metting.Models;

namespace Mobet.Metting.Droid
{
    [Activity(Label = "Óö¼û", NoHistory = true, Theme = "@style/Theme.DeviceDefaultLightNoActionBar")]
    public class LoginActivity : MvxActivity<LoginModel>
    {
        protected override void OnViewModelSet()
        {
            Window.AddFlags(WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(WindowManagerFlags.TranslucentNavigation);
            base.OnViewModelSet();
            SetContentView(Resource.Layout.Login);
        }
    }
}