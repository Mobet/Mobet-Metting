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

using Mobet.Droid.Components.ResideMenu.Activities;
using Mobet.Droid.Components.ResideMenu.Sample.Fragments;
using Mobet.Droid.Components.ResideMenu.Enums;

namespace Mobet.Droid.Components.ResideMenu.Sample.Activitys
{
    [Activity(Label = "Fragment Change", Theme = "@style/DefaultTheme")]
    public class FragmentChangeActivity : ResideFragmentActivity
    {

        private Android.Support.V4.App.Fragment content;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Android.Resource.Style.ThemeDeviceDefaultLightNoActionBar);

            Window.AddFlags(WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(WindowManagerFlags.TranslucentNavigation);
            base.OnCreate(savedInstanceState);

            SetBehindContentView(Resource.Layout.menu_frame_common);


            if (null != savedInstanceState)
                content = SupportFragmentManager.GetFragment(savedInstanceState, "content");
            if (null == content)
                content = new ColorFragment(Resource.Color.red);

            SetContentView(Resource.Layout.content_frame_common);
            SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.content_common_fragment, content)
                .Commit();

            SetBehindContentView(Resource.Layout.menu_frame_common);
            SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.menu_fragment_common, new ColorMenuFragment())
                .Commit();

            ResideMenu.ShadowWidthRes = Resource.Dimension.shadow_width;
            ResideMenu.BehindOffsetRes = Resource.Dimension.residemenu_offset;
            ResideMenu.ShadowDrawableRes = Resource.Drawable.shadow;
            ResideMenu.FadeDegree = 0.25f;
            ResideMenu.TouchModeAbove = TouchMode.Fullscreen;

        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            SupportFragmentManager.PutFragment(outState, "content", content);
        }
        public void SwitchContent(Android.Support.V4.App.Fragment fragment)
        {
            content = fragment;
            SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.content_common_fragment, fragment)
                .Commit();
            ResideMenu.ShowContent();
        }
    }


}