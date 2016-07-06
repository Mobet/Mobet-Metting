using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Mobet.Metting.Droid
{
    [Activity(Label = "遇见", MainLauncher = true, NoHistory = true, Theme = "@style/Theme.DeviceDefaultLightNoActionBar")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // 设置线程等待
            //Thread.Sleep(2000);

            //启动MainActivity
            StartActivity(typeof(ADActivity));
        }
    }
}