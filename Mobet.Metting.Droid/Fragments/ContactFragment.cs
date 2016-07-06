using Android.OS;
using Android.Views;

using SupportFragment = Android.Support.V4.App.Fragment;

namespace Mobet.Metting.Droid.Fragments
{
    public class ContactFragment : SupportFragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup root, Bundle data)
        {
            return inflater.Inflate(Resource.Layout.main_frame_contact, null, false);
        }
    }
}