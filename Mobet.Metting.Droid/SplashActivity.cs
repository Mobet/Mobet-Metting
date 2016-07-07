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

namespace Mobet.Metting.Droid
{
    [Activity(Label = "遇见", MainLauncher = true, NoHistory = true, Theme = "@style/Theme.DeviceDefaultLightNoActionBarFullScreen")]
    public class SplashActivity : Activity //, ViewPager.IOnPageChangeListener
    {
        //private  ImageView[] mImageViews;
        //public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        //{
        //    //throw new NotImplementedException();
        //}

        //public void OnPageScrollStateChanged(int state)
        //{
        //    //throw new NotImplementedException();
        //}

        //public void OnPageSelected(int position)
        //{
        //    //throw new NotImplementedException();
        //}

        //protected override void OnCreate(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);
        //    // 设置线程等待
        //    //Thread.Sleep(2000);
        //    SetContentView(Resource.Layout.splash);
        //    ViewGroup group = (ViewGroup)FindViewById(Resource.Id.splash_viewgroup);
        //    ViewPager pager = (ViewPager)FindViewById(Resource.Id.splash_viewpager);

        //    //载入图片资源ID  
        //    var imgIdArray = new int[]{Resource.Drawable.splash_1, Resource.Drawable.splash_2, Resource.Drawable.splash_3, Resource.Drawable.splash_4, Resource.Drawable.splash_5 };


        //    //将点点加入到ViewGroup中  
        //    var tips = new ImageView[imgIdArray.Length];
        //    for (int i = 0; i < tips.Length; i++)
        //    {
        //        ImageView imageView = new ImageView(this);
        //        //imageView.LayoutParameters = new ViewGroup.LayoutParams(10, 10);
        //        //tips[i] = imageView;
        //        //if (i == 0)
        //        //{
        //        //    tips[i].SetBackgroundResource(Resource.Drawable.page_indicator_focused);
        //        //}
        //        //else {
        //        //    tips[i].SetBackgroundResource(Resource.Drawable.page_indicator_unfocused);
        //        //}

        //        //LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(new ViewGroup.LayoutParams(LayoutParams.WRAP_CONTENT,
        //        //        LayoutParams.WRAP_CONTENT));
        //        //layoutParams.leftMargin = 5;
        //        //layoutParams.rightMargin = 5;


        //        group.AddView(imageView);
        //    }


        //    //将图片装载到数组中  
        //     mImageViews = new ImageView[imgIdArray.Length];
        //    for (int i = 0; i < mImageViews.Length; i++)
        //    {
        //        ImageView imageView = new ImageView(this);
        //        mImageViews[i] = imageView;
        //        imageView.SetBackgroundResource(imgIdArray[i]);
        //    }

        //    //设置Adapter  
        //    pager.Adapter = new MyAdapter();
        //    //设置监听，主要是设置点点的背景  
        //    pager.SetOnPageChangeListener(this);
        //    //设置ViewPager的默认项, 设置为长度的100倍，这样子开始就能往左滑动  
        //    pager.CurrentItem = ((mImageViews.Length) * 100);
        //    this.FindViewById<Button>(Resource.Id.button_splash_skip)
        //        .Click += (sender, args) => { StartActivity(typeof(LoginActivity)); };
        //}

        //public class MyAdapter : PagerAdapter
        //{
        //    private ImageView[] mImageViews;

        //    public MyAdapter() {
        //        //载入图片资源ID  
        //        var imgIdArray = new int[] { Resource.Drawable.splash_1, Resource.Drawable.splash_2, Resource.Drawable.splash_3, Resource.Drawable.splash_4, Resource.Drawable.splash_5 };

        //        //将图片装载到数组中  
        //        mImageViews = new ImageView[imgIdArray.Length];
        //        for (int i = 0; i < mImageViews.Length; i++)
        //        {
        //            ImageView imageView = new ImageView(this);
        //            mImageViews[i] = imageView;
        //            imageView.SetBackgroundResource(imgIdArray[i]);
        //        }

        //    }
        //    public override int Count
        //    {
        //        get
        //        {
        //            return 5;
        //        }
        //    }

        //    public override bool IsViewFromObject(View view, Java.Lang.Object objectValue)
        //    {
        //        return view == objectValue;
        //    }

        //    public override Java.Lang.Object InstantiateItem(View container, int position)
        //    {
        //        ((ViewPager)container).AddView(mImageViews[position % mImageViews.Length], 0);
        //        return mImageViews[position % mImageViews.Length];
        //    }
        //}
    }




}