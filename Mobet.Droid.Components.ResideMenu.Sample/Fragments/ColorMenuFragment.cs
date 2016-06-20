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
using Fragment = Android.Support.V4.App.Fragment;
using Mobet.Droid.Components.ResideMenu.Sample.Activitys;

namespace Mobet.Droid.Components.ResideMenu.Sample.Fragments
{
    public class ColorMenuFragment : Android.Support.V4.App.ListFragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup p1, Bundle p2)
        {
            return inflater.Inflate(Resource.Layout.menu_frame_color, null);
        }

        public override void OnActivityCreated(Bundle p0)
        {
            base.OnActivityCreated(p0);

            var colors = Resources.GetStringArray(Resource.Array.color_names);
            var colorAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleListItem1,
                                                        Android.Resource.Id.Text1, colors);
            ListAdapter = colorAdapter;
        }

        public override void OnListItemClick(ListView p0, View p1, int position, long p3)
        {
            Fragment fragment = null;
            switch (position)
            {
                case 0:
                    fragment = new ColorFragment(Resource.Color.red);
                    break;
                case 1:
                    fragment = new ColorFragment(Resource.Color.green);
                    break;
                case 2:
                    fragment = new ColorFragment(Resource.Color.blue);
                    break;
                case 3:
                    fragment = new ColorFragment(Resource.Color.white);
                    break;
                case 4:
                    fragment = new ColorFragment(Resource.Color.black);
                    break;
            }
            var activity = Activity as FragmentChangeActivity;
            if (activity != null)
                activity.SwitchContent(fragment);
        }
    }
}