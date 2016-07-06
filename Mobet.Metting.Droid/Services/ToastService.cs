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
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

using Mobet.Metting.UI;


namespace Mobet.Metting.Droid.Services
{
    public class ToastService : IToastService
    {
        public void Alert(string message)
        { 
            var topActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            var context = topActivity.Activity;

            Toast
                .MakeText(context,message,ToastLength.Long)
                .Show();
        }
    }
}