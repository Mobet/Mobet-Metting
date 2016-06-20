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

namespace Mobet.Droid.Components.ResideMenu.Sample.Fragments
{
    public class ColorFragment : Android.Support.V4.App.Fragment
    {
        private int colorResourceId = -1;

        public ColorFragment()
            : this(Resource.Color.white)
        { }

        public ColorFragment(int colorRes)
        {
            this.colorResourceId = colorRes;
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (null != savedInstanceState)
                colorResourceId = savedInstanceState.GetInt("colorResourceId");
            var color = Resources.GetColor(colorResourceId);
            var v = new RelativeLayout(Activity);
            v.SetBackgroundColor(color);
            return v;
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutInt("colorResourceId", colorResourceId);
        }
    }
}