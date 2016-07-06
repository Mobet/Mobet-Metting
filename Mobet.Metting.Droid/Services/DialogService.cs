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
using Mobet.Metting.UI;

using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace Mobet.Metting.Droid.Services
{
    public class DialogService : IDialogService
    {
        public void Alert(string message, string title, string OkButtonText)
        { 

            var topActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();

            var context = topActivity.Activity;

            var adb = new AlertDialog.Builder(context);

            adb.SetTitle(title);
            adb.SetMessage(message);
            adb.SetPositiveButton(OkButtonText, (sender, args) => { });

            adb.Create().Show();
        }
    }
}