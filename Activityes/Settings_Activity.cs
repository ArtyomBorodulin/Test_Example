using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using TextColor = Android.Content.Res.ColorStateList;
using Test_Example.Change_color;

namespace Test_Example
{
    [Activity(Label = "Settings_Activity")]
    public class Settings_Activity : Activity
    {
        string selected_item;
        public MyListView1 mylist1;
        int count = 0;
        public ListView listView;
        public LinearLayout layout;
        public TextView[] labels = new TextView[1];
        TextView text;
        public string[] result;
        bool stop = false;
        List<string> Strings;
        ISharedPreferences pref;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Settings_Page);
            pref = Application.GetSharedPreferences("backgroundColor_textColor", FileCreationMode.Private);
            result = new string[] { pref.GetString("background", "White"), pref.GetString("textColor", "Black") };
            listView = FindViewById<ListView>(Resource.Id.listView1);
            Strings = new List<string>();
            Strings.Add("Внешний вид");
            Strings.Add("Экраны данных");
            Strings.Add("Подключить датчик ЧП");
            mylist1 = new MyListView1(this, Strings, Color.ParseColor(result[1]));
            text = FindViewById<TextView>(Resource.Id.title_settings);


            listView.Adapter = mylist1;
            listView.SetBackgroundColor(Color.White);

            labels[0] = FindViewById<TextView>(Resource.Id.background_value);
            labels[0].Text = result[0];
            labels[0].TextChanged += Settings_Activity_TextChanged;



            layout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            listView.ItemClick += ListView_ItemClick;
            layout.SetBackgroundColor(Color.ParseColor(result[0]));
            text.SetTextColor(Color.ParseColor(result[1]));
            Task.Run(async () => await Update_Screen());
        }

        private void Settings_Activity_TextChanged(object sender, Android.Text.TextChangedEventArgs e) // background color was change
        {
            if(labels[0].Text == "Black")
            {
                listView.Adapter = new MyListView1(this,Strings,Color.White); //Color.White - Цвет нашего текста
                listView.SetBackgroundColor(Color.Black);
            }
            else
            {
                listView.Adapter = new MyListView1(this,Strings,Color.Black);
                listView.SetBackgroundColor(Color.White);
            }
        }

        private async Task Update_Screen()
        {
            while (!stop)
            {
                if(count == 0)
                {
                    await Task.Delay(1);
                    count++;
                }
                else
                  await Task.Delay(400);
                result = new string[] { pref.GetString("background", "White"), pref.GetString("textColor", "Black") };
                RunOnUiThread(() =>
                {
                    layout.SetBackgroundColor(Color.ParseColor(result[0]));
                    listView.Adapter = new MyListView1(this, Strings, Color.ParseColor(result[1]));
                    text.SetTextColor(Color.ParseColor(result[1]));
                    labels[0].Text = result[0];
                });
            }
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            selected_item = (string)listView.GetItemAtPosition(e.Position);
            switch (selected_item)
            {
                case "Внешний вид":
                    Intent intent = new Intent(this, typeof(font_settings_activity));
                    StartActivity(intent);
                    break;
                case "Экраны данных":
                    Toast.MakeText(this, "Экраны данных находятся в разработке", ToastLength.Short).Show();
                    break;
                case "Подключить датчик ЧП":
                    Intent intent2 = new Intent(this, typeof(Activityes.Connect_SensorActivity));
                    StartActivity(intent2);
                    break;
            }
        }
    }
}