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
using Android.Content.PM;

[assembly: Permission(Name = Mobet.Metting.Droid.Application.JPUSH_MESSAGE_PERMISSION, ProtectionLevel = Protection.Signature)]
[assembly: UsesPermission(Name = Mobet.Metting.Droid.Application.JPUSH_MESSAGE_PERMISSION)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.WakeLock)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.Vibrate)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.ReadPhoneState)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.WriteExternalStorage)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.ReadExternalStorage)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.MountUnmountFilesystems)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.AccessNetworkState)]
[assembly: UsesPermission(Name = "android.permission.RECEIVE_USER_PRESENT")]
[assembly: UsesPermission(Name = Android.Manifest.Permission.WriteSettings)]
namespace Mobet.Metting.Droid
{
    [Application]
    [MetaData("JPUSH_CHANNEL", Value = "developer-default")]
    [MetaData("JPUSH_APPKEY", Value = "7ca59af157d3eb61c3d3f18b")]
    public class Application : Android.App.Application
    {
        public const string JPUSH_MESSAGE_PERMISSION = "com.mobet.metting.permission.JPUSH_MESSAGE";

        public Application(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

        }
    }
}