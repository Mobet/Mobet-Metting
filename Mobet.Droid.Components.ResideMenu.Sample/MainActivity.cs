using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Mobet.Droid.Components.ResideMenu.Sample.Activitys;

namespace Mobet.Droid.Components.ResideMenu.Sample
{
    [Activity(Label = "Mobet.Droid.Components.ResideMenu.Sample", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/DefaultTheme")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.main);


            this.FindViewById<Button>(Resource.Id.main_btn_changing_fragments)
                .Click += Button_Changing_Fragments_Click; ;
            
        }

        private void Button_Changing_Fragments_Click(object sender, EventArgs e)
        {
            StartActivity(new Intent(this, typeof(FragmentChangeActivity)));
        }
    }
}

