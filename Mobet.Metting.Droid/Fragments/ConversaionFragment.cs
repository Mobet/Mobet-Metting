using System;
using Android.OS;
using Android.Views;
using Android.Widget;

using SupportListFragment = Android.Support.V4.App.ListFragment;

using Mobet.Droid.Components.AmazingRefresharp.Views;
using Mobet.Droid.Components.AmazingRefresharp.SwipeMenuList;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Util;
using Mobet.Metting.Droid.Adapters;

namespace Mobet.Metting.Droid.Fragments
{
    public class ConversaionFragment : SupportListFragment, ISwipeMenuCreator, IOnMenuItemClickListener
    {
        private IAmazingRefreshsharpView ptr_view;


        public bool FastScrollEnabled;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup root, Bundle data)
        {
            var view = inflater.Inflate(Resource.Layout.main_frame_conversation, null, false);

            if (view != null)
            {
            }
            return view;
        }

        public override void OnViewStateRestored(Bundle savedInstanceState)
        {
            base.OnViewStateRestored(savedInstanceState);

            var lv = ListView as Mobet.Droid.Components.AmazingRefresharp.Widget.ListView;
            if (lv != null)
            {
                lv.SetMenuCreator(this);
                lv.SetOnMenuItemClickListener(this);
            }

            ListView.FastScrollEnabled = FastScrollEnabled;

            if (ptr_view == null && ListView is IAmazingRefreshsharpView)
            {
                ptr_view = (IAmazingRefreshsharpView)ListView;
                // LOOK HERE!
                // Hookup a handler to the RefreshActivated event
                ptr_view.RefreshActivated += ptr_view_RefreshActivated;
            }
            ListAdapter = new ConversaionAdapter(this.Activity, new Conversation[] {
                new Conversation {Name = "穆轻寒",Say ="青春不是年华,而是心境", Time="昨天" }
            });
        }

        private void ptr_view_RefreshActivated(object sender, EventArgs args)
        {
            // LOOK HERE!
            // Refresh your content when PullToRefresharp informs you that a refresh is needed
            View.PostDelayed(() => {
                if (ptr_view != null)
                {
                    // When you are done refreshing your content, let PullToRefresharp know you're done.
                    ptr_view.OnRefreshCompleted();
                }
            }, 2000);
        }

        public override void OnDestroyView()
        {
            if (ptr_view != null)
            {
                ptr_view.RefreshActivated -= ptr_view_RefreshActivated;
                ptr_view = null;
            }

            base.OnDestroyView();
        }

        public override void OnResume()
        {
            base.OnResume();
            ListView.ItemClick += listview_ItemClick;
        }

        public override void OnPause()
        {
            base.OnPause();
            ListView.ItemClick -= listview_ItemClick;
        }

        private void listview_ItemClick(object sender, AdapterView.ItemClickEventArgs args)
        {
            Toast.MakeText(Activity, args.Position + " Clicked", ToastLength.Short).Show();
        }

        public void Create(SwipeMenu menu)
        {
            SwipeMenuItem item = new SwipeMenuItem(Activity.BaseContext);
            item.Title = "Item 1";
            item.Background = new ColorDrawable(Color.Gray);
            item.Width = Dp2Px(100);
            menu.AddMenuItem(item);

            item = new SwipeMenuItem(Activity.BaseContext);
            item.Title = "Item 2";
            item.Background = new ColorDrawable(Color.Red);
            item.Width = Dp2Px(100);
            menu.AddMenuItem(item);
        }

        private int Dp2Px(int dp)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp,
                Activity.BaseContext.Resources.DisplayMetrics);
        }

        public bool OnMenuItemClick(int position, SwipeMenu menu, int index)
        {
            Toast.MakeText(Activity.BaseContext, "The Position is " + position + " And Index Is" + index, ToastLength.Short).Show();
            return true;
        }
    }
}
