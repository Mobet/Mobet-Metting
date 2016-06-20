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
using Mobet.Droid.Components.ResideMenu;

namespace Mobet.Droid.Components.ResideMenu.Activities
{
    public interface IResideActivity
    {
        void SetBehindContentView(View view, ViewGroup.LayoutParams layoutParams);
        void SetBehindContentView(View view);
        void SetBehindContentView(int layoutResId);
        ResideMenuLayout ResideMenu { get; }
        void Toggle();
        void ShowContent();
        void ShowMenu();
        void ShowSecondaryMenu();
        void SetResideActionBarEnabled(bool enabled);
    }
}