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
using Android.Graphics;

namespace Test_Example.Change_color
{
    public class MyListView1 : BaseAdapter<string>
    {
        public List<string> MItems;
        public Context mContext;
        public Color color;
        public MyListView1(Context context, List<string> Items, Color color)
        {
            mContext = context;
            MItems = Items;
            this.color = color;
        }

        public override int Count => MItems.Count;
        public override string this[int position] => MItems[position];
        public override long GetItemId(int position)
        {
            return position;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.Text, null, false);
            }
            TextView textView = row.FindViewById<TextView>(Resource.Id.Black);
            textView.Text = MItems[position];
            textView.SetTextColor(color);
            return row;
        }
    }
}