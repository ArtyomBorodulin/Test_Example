using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using TextColor = Android.Content.Res.ColorStateList;

namespace Test_Example
{
    [Activity(Label = "font_settings_activity")]
    public class font_settings_activity : Activity
    {
        RadioGroup font_list;
        ISharedPreferences pref;
        LinearLayout layout1;
        RadioButton[] buttons = new RadioButton[2];
        TextView text;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.font_settings_page);
            pref = Application.GetSharedPreferences("backgroundColor_textColor", FileCreationMode.Private);
            string background = pref.GetString("background", "White");
            string textColor = pref.GetString("textColor", "Black");
            font_list = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
            buttons[0] = FindViewById<RadioButton>(Resource.Id.radioButton1);
            buttons[1] = FindViewById<RadioButton>(Resource.Id.radioButton2);
            text = FindViewById<TextView>(Resource.Id.textView1);
            layout1 = FindViewById<LinearLayout>(Resource.Id.linearLayout3);

            layout1.SetBackgroundColor(Color.ParseColor(background));
            Change_color_radioButton(Resource.Id.radioButton1, textColor);
            Change_color_radioButton(Resource.Id.radioButton2, textColor);
            text.SetTextColor(Color.ParseColor(textColor));
            if(background == "Black")
            {
                buttons[1].Checked = true;
            }
            else
            {
                buttons[0].Checked = true;
            }
            font_list.CheckedChange += Font_list_CheckedChange;
        }

        private void Font_list_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            var edit = pref.Edit();
            switch(font_list.CheckedRadioButtonId)
            {
                case Resource.Id.radioButton1: // Чёрный текст, белый фон
                    string[] mass = new string[] { "Black", "White" };
                    layout1.SetBackgroundColor(Color.White);
                    Change_color_radioButton(Resource.Id.radioButton1, mass[0]);
                    Change_color_radioButton(Resource.Id.radioButton2, mass[0]);
                    text.SetTextColor(Android.Content.Res.ColorStateList.ValueOf(Color.Black));
                    edit.PutString("background", mass[1]);
                    edit.PutString("textColor", mass[0]);
                    edit.Commit();
                    break;
                case Resource.Id.radioButton2:
                    string[] mass2 = new string[] { "White", "Black" };
                    layout1.SetBackgroundColor(Color.Black);
                    Change_color_radioButton(Resource.Id.radioButton1, mass2[0]);
                    Change_color_radioButton(Resource.Id.radioButton2, mass2[0]);
                    text.SetTextColor(Android.Content.Res.ColorStateList.ValueOf(Color.White));
                    edit.PutString("background", mass2[1]);
                    edit.PutString("textColor", mass2[0]);
                    edit.Commit();
                    break;
            }
        }

        private void Change_color_radioButton(int id, string color)
        {
            RadioButton button = FindViewById<RadioButton>(id);
            button.SetTextColor(Android.Content.Res.ColorStateList.ValueOf(Color.ParseColor(color)));
            button.ButtonTintList = Android.Content.Res.ColorStateList.ValueOf(Color.ParseColor(color));
        }
    }
}