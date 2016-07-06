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
using Android.Support.V4.App;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;

namespace Mobet.Droid.Components.ResideMenu.Activities
{
    public class ResideFragmentActivity : FragmentActivity, IResideActivity
    {
        private ResideActivityHelper helper;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            helper = new ResideActivityHelper(this);
            helper.OnCreate(savedInstanceState);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            helper.OnPostCreate(savedInstanceState);
        }

        public override View FindViewById(int id)
        {
            var v = base.FindViewById(id);
            return v ?? helper.FindViewById(id);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            helper.OnSaveInstanceState(outState);
        }

        public override void SetContentView(int layoutResId)
        {
            SetContentView(LayoutInflater.Inflate(layoutResId, null));
        }

        public override void SetContentView(View view)
        {
            SetContentView(view, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent));
        }

        public override void SetContentView(View view, ViewGroup.LayoutParams @params)
        {
            base.SetContentView(view, @params);
            helper.RegisterAboveContentView(view, @params);
        }

        public void SetBehindContentView(View view, ViewGroup.LayoutParams layoutParams)
        {
            helper.SetBehindContentView(view, layoutParams);
        }

        public void SetBehindContentView(View view)
        {
            SetBehindContentView(view, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent));
        }

        public void SetBehindContentView(int layoutResId)
        {
            SetBehindContentView(LayoutInflater.Inflate(layoutResId, null));
        }

        public ResideMenuLayout ResideMenu
        {
            get { return helper.ResideMenu; }
        }

        public void Toggle()
        {
            helper.Toggle();
        }

        public void ShowContent()
        {
            helper.ShowContent();
        }

        public void ShowMenu()
        {
            helper.ShowMenu();
        }

        public void ShowSecondaryMenu()
        {
            helper.ShowSecondaryMenu();
        }

        public void SetResideActionBarEnabled(bool enabled)
        {
            helper.ResideActionBarEnabled = enabled;
        }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
            var b = helper.OnKeyUp(keyCode, e);
            return b ? b : base.OnKeyUp(keyCode, e);
        }
    }
  
    public class MvxResideFragmentActivity<TViewModel> : MvxFragmentActivity<TViewModel>, IResideActivity
        where TViewModel : class, IMvxViewModel
    {
        private ResideActivityHelper helper;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            helper = new ResideActivityHelper(this);
            helper.OnCreate(savedInstanceState);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            helper.OnPostCreate(savedInstanceState);
        }

        public override View FindViewById(int id)
        {
            var v = base.FindViewById(id);
            return v ?? helper.FindViewById(id);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            helper.OnSaveInstanceState(outState);
        }

        public override void SetContentView(int layoutResId)
        {
            SetContentView(LayoutInflater.Inflate(layoutResId, null));
        }

        public override void SetContentView(View view)
        {
            SetContentView(view, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent));
        }

        public override void SetContentView(View view, ViewGroup.LayoutParams @params)
        {
            base.SetContentView(view, @params);
            helper.RegisterAboveContentView(view, @params);
        }

        public void SetBehindContentView(View view, ViewGroup.LayoutParams layoutParams)
        {
            helper.SetBehindContentView(view, layoutParams);
        }

        public void SetBehindContentView(View view)
        {
            SetBehindContentView(view, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent));
        }

        public void SetBehindContentView(int layoutResId)
        {
            SetBehindContentView(LayoutInflater.Inflate(layoutResId, null));
        }

        public ResideMenuLayout ResideMenu
        {
            get { return helper.ResideMenu; }
        }

        public void Toggle()
        {
            helper.Toggle();
        }

        public void ShowContent()
        {
            helper.ShowContent();
        }

        public void ShowMenu()
        {
            helper.ShowMenu();
        }

        public void ShowSecondaryMenu()
        {
            helper.ShowSecondaryMenu();
        }

        public void SetResideActionBarEnabled(bool enabled)
        {
            helper.ResideActionBarEnabled = enabled;
        }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
            var b = helper.OnKeyUp(keyCode, e);
            return b ? b : base.OnKeyUp(keyCode, e);
        }
    }
}