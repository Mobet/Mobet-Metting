using Android.OS;
using Android.Views;

namespace Mobet.Metting.Droid.Fragments
{
    public class SettingFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.setting, null,false);
        }
    }
}