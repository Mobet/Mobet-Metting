using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Mobet.Metting.Droid.Fragments
{
    public class ResideMenuFragment : Android.Support.V4.App.Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.menu_frame_personal, container, false);

            view.FindViewById<TextView>(Resource.Id.menu_frame_personal_button_setting)
                .Click += delegate (object sender, EventArgs e)
                 {
                     StartActivity(new Intent(this.Context, typeof(SettingActivity)));
                 };
            return view;
        }
    }
}