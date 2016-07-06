using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using MvvmCross.Droid.Views;
using Mobet.Metting.Droid.Fragments;
using Mobet.Metting.Models;
using MvvmCross.Droid.Support.V4;
using Mobet.Droid.Components.AmazingRefresharp.Views;
using Mobet.Droid.Components.ResideMenu.Activities;
using Mobet.Droid.Components.ResideMenu.Enums;

namespace Mobet.Metting.Droid
{
    [Activity(Label = "遇见")]
    public class MainActivity : MvxResideFragmentActivity<MainModel>
    {
        private Android.Support.V4.App.Fragment content;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Android.Resource.Style.ThemeDeviceDefaultLightNoActionBar);

            Window.AddFlags(WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(WindowManagerFlags.TranslucentNavigation);

            //Window.SetStatusBarColor(TitleColor);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.main);

            SetBehindContentView(Resource.Layout.main);
            this.SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.main_frame_content, new ConversaionFragment())
                .Commit(); 

            SetBehindContentView(Resource.Layout.menu_frame_layout);
            this.SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.menu_fragment_common, new ResideMenuFragment())
                .Commit();

            ResideMenu.ShadowWidthRes = Resource.Dimension.shadow_width;
            ResideMenu.BehindOffsetRes = Resource.Dimension.reside_menu_offset;
            ResideMenu.ShadowDrawableRes = Resource.Drawable.reside_menu_shadow;
            ResideMenu.FadeDegree = 0.25f;
            ResideMenu.TouchModeAbove = TouchMode.Margin;


            this.FindViewById<ImageView>(Resource.Id.main_frame_footer_conversation)
                .Click += delegate (object sender, EventArgs e)
                {
                    InitialFooterIconState();
                    ((ImageView)sender).SetImageResource(Resource.Drawable.skin_tab_icon_conversation_selected);
                    this.SupportFragmentManager
                        .BeginTransaction()
                        .Replace(Resource.Id.main_frame_content, new ConversaionFragment())
                        .Commit();
                };

            this.FindViewById<ImageView>(Resource.Id.main_frame_footer_call)
               .Click += delegate (object sender, EventArgs e)
               {
                   InitialFooterIconState();
                   ((ImageView)sender).SetImageResource(Resource.Drawable.skin_tab_icon_call_selected);
                   this.SupportFragmentManager
                       .BeginTransaction()
                       .Replace(Resource.Id.main_frame_content, new SettingFragment())
                       .Commit();
               };

            this.FindViewById<ImageView>(Resource.Id.main_frame_footer_contact)
               .Click += delegate (object sender, EventArgs e)
               {
                   InitialFooterIconState();
                   ((ImageView)sender).SetImageResource(Resource.Drawable.skin_tab_icon_contact_selected);
                   this.SupportFragmentManager
                       .BeginTransaction()
                       .Replace(Resource.Id.main_frame_content, new ContactFragment())
                       .Commit();
               };

            this.FindViewById<ImageView>(Resource.Id.main_frame_footer_plugin)
               .Click += delegate (object sender, EventArgs e)
               {
                   InitialFooterIconState();
                   ((ImageView)sender).SetImageResource(Resource.Drawable.skin_tab_icon_plugin_selected);
                   this.SupportFragmentManager
                       .BeginTransaction()
                       .Replace(Resource.Id.main_frame_content, new ConversaionFragment())
                       .Commit();
               };

        }

        private void InitialFooterIconState()
        {
            this.FindViewById<ImageView>(Resource.Id.main_frame_footer_conversation)
                .SetImageResource(Resource.Drawable.skin_tab_icon_conversation_normal);
            this.FindViewById<ImageView>(Resource.Id.main_frame_footer_call)
                .SetImageResource(Resource.Drawable.skin_tab_icon_call_normal);
            this.FindViewById<ImageView>(Resource.Id.main_frame_footer_contact)
                .SetImageResource(Resource.Drawable.skin_tab_icon_contact_normal);
            this.FindViewById<ImageView>(Resource.Id.main_frame_footer_plugin)
                .SetImageResource(Resource.Drawable.skin_tab_icon_plugin_normal);
        }
        public void SwitchContent(Android.Support.V4.App.Fragment fragment)
        {
            this.content = fragment;
            SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.main_frame_content, fragment)
                .Commit();
            ResideMenu.ShowContent();
        }
    }
}

