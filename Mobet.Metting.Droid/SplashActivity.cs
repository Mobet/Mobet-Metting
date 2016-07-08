using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Java.Lang;
using CN.Jpush.Android.Api;

namespace Mobet.Metting.Droid
{
    [Activity(Label = "遇见", MainLauncher = true, NoHistory = true, Theme = "@style/Theme.DeviceDefaultLightNoActionBar")]
    public class SplashActivity : Activity, ViewPager.IOnPageChangeListener
    {
        private ImageView[] imageViews;
        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
        }
        public void OnPageScrollStateChanged(int state)
        {
        }
        public void OnPageSelected(int position)
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {

            Window.AddFlags(WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(WindowManagerFlags.TranslucentNavigation);

            base.OnCreate(savedInstanceState);

            

            JPushInterface.SetDebugMode(true);
            JPushInterface.Init(this);

            SetContentView(Resource.Layout.splash);
            ViewPager pager = (ViewPager)FindViewById(Resource.Id.splash_viewpager);

            //载入图片资源ID  
            var imgIdArray = new int[] { Resource.Drawable.menu_bg_blue, Resource.Drawable.menu_bg_dark, Resource.Drawable.menu_bg_night };

            //将图片装载到数组中  
            imageViews = new ImageView[imgIdArray.Length];
            for (int i = 0; i < imageViews.Length; i++)
            {
                ImageView imageView = new ImageView(this);
                imageView.SetBackgroundResource(imgIdArray[i]);
                imageViews[i] = imageView;
            }
            //设置Adapter  
            pager.Adapter = new SplashViewPagerAdapter(imageViews);
            //设置监听，主要是设置点点的背景  

            this.FindViewById<Button>(Resource.Id.button_splash_skip)
                .Click += (sender, args) => { StartActivity(typeof(LoginActivity)); };
        }

        public class SplashViewPagerAdapter : PagerAdapter
        {
            private ImageView[] imageViews;

            public SplashViewPagerAdapter(ImageView[] imageViews)
            {
                this.imageViews = imageViews;
            }
            public override int Count
            {
                get
                {
                    return imageViews.Length;
                }
            }

            public override bool IsViewFromObject(View view, Java.Lang.Object objectValue)
            {
                return view == objectValue;
            }

            public override Java.Lang.Object InstantiateItem(View container, int position)
            {
                var image = imageViews[position % imageViews.Length];

                if (image.Parent != null)
                {
                    ((ViewGroup)image.Parent).RemoveView(image);
                }

                ((ViewPager)container).AddView(image, 0);
                return image;
            }

            public override void DestroyItem(View container, int position, Java.Lang.Object objectValue)
            {
                ((ViewGroup)container).RemoveView((View)objectValue);
                objectValue = null;
            }
        }
    }
}