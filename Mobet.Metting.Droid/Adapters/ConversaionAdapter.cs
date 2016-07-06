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

namespace Mobet.Metting.Droid.Adapters
{
    public class Conversation
    {
        public string Name { get; set; }
        public string Say { get; set; }
        public string Time { get; set; }
    }
    public class ConversaionAdapter : BaseAdapter<Conversation>
    {
        Conversation[] items;
        Activity activity;

        public ConversaionAdapter(Activity context, Conversation[] values)
            : base()
        {
            activity = context;
            items = values;
        }

        public override Conversation this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
            get { return items.Length; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View v = convertView;
            if (v == null)
                v = activity.LayoutInflater.Inflate(Resource.Layout.main_frame_conversation_item, null);

            v.FindViewById<TextView>(Resource.Id.main_conversation_name).Text = items[position].Name;
            v.FindViewById<TextView>(Resource.Id.main_conversation_say).Text = items[position].Say;
            v.FindViewById<TextView>(Resource.Id.main_conversation_time).Text = items[position].Time;

            return v;
        }
    }
}