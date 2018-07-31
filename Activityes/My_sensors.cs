using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.Design;
using Android.Support.V7.Widget;
using Android.Views;
using Test_Example.Change_color;
using Android.Widget;
using V7ToolBar = Android.Support.V7.Widget.Toolbar;
using BarLayout = Android.Support.Design.Widget.AppBarLayout;
using Android.Graphics;


namespace Test_Example.Activityes
{
    [Activity(Label = "My_sensors", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class My_sensors : AppCompatActivity
    {
        TextView Titlenew;
        V7ToolBar toolBar;
        List<string> result;
        string text_color, background;
        ListView my_sensors;
        string selected;
        LinearLayout linearLayout;
        ICollection<string> objects;
        BarLayout barLayout;
        ISharedPreferences pref,background_color;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.my_sensors);
            Titlenew = FindViewById<TextView>(Resource.Id.title_my_sensors);
            linearLayout = FindViewById<LinearLayout>(Resource.Id.my_sensors_layout);
            my_sensors = FindViewById<ListView>(Resource.Id.my_sensors_listView);
            barLayout = FindViewById<BarLayout>(Resource.Id.appBarLayout);
            pref = GetSharedPreferences("sensors", FileCreationMode.Private);
            background_color = GetSharedPreferences("backgroundColor_textColor", FileCreationMode.Private);
            text_color = background_color.GetString("textColor", "Black");
            background = background_color.GetString("background", "White");

            objects = pref.GetStringSet("sensor", null);
            if (objects != null)
            {
                result = objects.ToList();
                MyListView1 mylist = new MyListView1(this, result, Color.ParseColor(text_color));
                my_sensors.Adapter = mylist;
                my_sensors.ItemLongClick += My_sensors_ItemLongClick;
            }
            else
            {
                Titlenew.Text = "Нет подключённых устройств";
            }
            linearLayout.SetBackgroundColor(Color.ParseColor(background));
            my_sensors.SetBackgroundColor(Color.ParseColor(background));
            Titlenew.SetTextColor(Color.ParseColor(text_color));

        }
        private void My_sensors_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            selected = my_sensors.GetItemAtPosition(e.Position).ToString();
            Toast.MakeText(this, "Длинное нажатие", ToastLength.Short).Show();
            LinearLayout.LayoutParams toolBarParams = new LinearLayout.LayoutParams(V7ToolBar.LayoutParams.MatchParent, 150);
            LinearLayout.LayoutParams appBarParams = new LinearLayout.LayoutParams(BarLayout.LayoutParams.MatchParent, Resource.Attribute.actionBarSize);
            toolBar = new V7ToolBar(this);

            toolBar.LayoutParameters = toolBarParams;
            toolBar.SetBackgroundColor(Color.Black);
            toolBar.SetTitle(Resource.String.my_sensors_toolbar_title);
            toolBar.SetTitleTextColor(Color.White);
            toolBar.Visibility = ViewStates.Visible;
            toolBar.PopupTheme = Resource.Style.AppTheme_PopupOverlay;
            barLayout.AddView(toolBar, 0);
            SetSupportActionBar(toolBar);

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.my_sensors_toolbar,menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var id = item.ItemId;
            if(id == Resource.Id.my_sensors_layout)
            {
                result.Remove(selected);
                MyListView1 myListView1 = new MyListView1(this, result,Color.ParseColor(text_color));
                my_sensors.Adapter = myListView1;
                barLayout.RemoveAllViews();
            }
            return true;
        }
    }
}