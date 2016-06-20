using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Mobet.Droid.Components.ResideMenu.Enums;
using System;
using System.Linq;
using System.Reflection;

namespace Mobet.Droid.Components.ResideMenu.Activities
{
    public class ResideActivityHelper
    {
        private readonly Activity activity;
        private ResideMenuLayout resideMenu;
        private View viewAbove;
        private View viewBehind;
        private bool broadcasting;
        private bool onPostCreateCalled;
        private bool enableSlide = true;
        public ResideActivityHelper(Activity activity)
        {
            ResourceIdManager.UpdateIdValues();
            this.activity = activity;
        }

        public void OnCreate(Bundle savedInstanceState)
        {
            resideMenu = (ResideMenuLayout)LayoutInflater.From(activity).Inflate(Resource.Layout.reside_menu_main, null);
        }

        public void OnPostCreate(Bundle savedInstanceState)
        {
            if (null == viewBehind && null == viewAbove)
                throw new InvalidOperationException("Both SetBehindContentView must be called " +
                    "in OnCreate in addition to SetContentView.");

            onPostCreateCalled = true;

            resideMenu.AttachToActivity(activity,
                enableSlide ? ResideStyle.Window : ResideStyle.Content);

            bool open, secondary;
            if (null != savedInstanceState)
            {
                open = savedInstanceState.GetBoolean("SlidingActivityHelper.open");
                secondary = savedInstanceState.GetBoolean("SlidingActivityHelper.secondary");
            }
            else
            {
                open = false;
                secondary = false;
            }

            new Handler().Post(() =>
            {
                if (open)
                {
                    if (secondary)
                        resideMenu.ShowSecondaryMenu(false);
                    else
                        resideMenu.ShowMenu(false);
                }
                else
                    resideMenu.ShowContent(false);
            });
        }

        public bool ResideActionBarEnabled
        {
            get { return enableSlide; }
            set
            {
                if (onPostCreateCalled)
                    throw new InvalidOperationException("EnableSlidingActionBar must be called in OnCreate.");
                enableSlide = value;
            }
        }

        public View FindViewById(int id)
        {
            if (resideMenu != null)
            {
                var v = resideMenu.FindViewById(id);
                if (v != null)
                    return v;
            }
            return null;
        }

        public void OnSaveInstanceState(Bundle outState)
        {
            outState.PutBoolean("SlidingActivityHelper.open", resideMenu.IsMenuShowing);
            outState.PutBoolean("SlidingActivityHelper.secondary", resideMenu.IsSecondaryMenuShowing);
        }

        public void RegisterAboveContentView(View v, ViewGroup.LayoutParams layoutParams)
        {
            if (broadcasting)
                viewAbove = v;
        }

        public void SetContentView(View v)
        {
            broadcasting = true;
            activity.SetContentView(v);
        }

        public void SetBehindContentView(View view, ViewGroup.LayoutParams layoutParams)
        {
            viewBehind = view;
            resideMenu.SetMenu(viewBehind);
        }

        public ResideMenuLayout ResideMenu
        {
            get { return resideMenu; }
        }

        public void Toggle()
        {
            resideMenu.Toggle();
        }

        public void ShowContent()
        {
            resideMenu.ShowContent();
        }

        public void ShowMenu()
        {
            resideMenu.ShowMenu();
        }

        public void ShowSecondaryMenu()
        {
            resideMenu.ShowSecondaryMenu();
        }

        public bool OnKeyUp(Keycode keycode, KeyEvent keyEvent)
        {
            if (keycode == Keycode.Back && resideMenu.IsMenuShowing)
            {
                ShowContent();
                return true;
            }
            return false;
        }
    }


    public static class ResourceIdManager
    {
        static bool idInitialized;
        public static void UpdateIdValues()
        {
            if (idInitialized)
                return;
            var eass = Assembly.GetExecutingAssembly();
            Func<Assembly, Type> f = ass =>
                ass.GetCustomAttributes(typeof(ResourceDesignerAttribute), true)
                    .Select(ca => ca as ResourceDesignerAttribute)
                    .Where(ca => ca != null && ca.IsApplication)
                    .Select(ca => ass.GetType(ca.FullName))
                    .Where(ty => ty != null)
                    .FirstOrDefault();
            var t = f(eass);
            if (t == null)
                t = AppDomain.CurrentDomain.GetAssemblies().Select(ass => f(ass)).Where(ty => ty != null).FirstOrDefault();
            if (t != null)
                t.GetMethod("UpdateIdValues").Invoke(null, new object[0]);
            idInitialized = true;
        }
    }
}