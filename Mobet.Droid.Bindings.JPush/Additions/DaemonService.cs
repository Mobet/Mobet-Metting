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

namespace CN.Jpush.Android.Service {

    [Service(Name = "cn.jpush.android.service.DaemonService", Enabled = true, Exported = true)]
    [IntentFilter(
        new string[] { "cn.jpush.android.intent.DaemonService" },
        Categories = new string[] { Constants.PACKAGE_NAME }
        )
    ]
    public partial class DaemonService {
    }
}