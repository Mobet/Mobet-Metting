
using Android.App;
using Android.OS;
using Android.Widget;

namespace Mobet.Metting.Droid
{
    [Activity(Label = "¹ã¸æ", NoHistory = true, Theme = "@style/Theme.DeviceDefaultLightNoActionBar")]
    public class ADActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ad);

            this.FindViewById<Button>(Resource.Id.button_ad_skip)
                .Click += (sender, args) =>{ StartActivity(typeof(LoginActivity)); };
        }
    }
}