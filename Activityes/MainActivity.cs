using System;
using Android;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using V7ToolBar = Android.Support.V7.Widget.Toolbar;
using Android.Graphics;
using Android.Content;
using Android.Content.Res;

namespace Test_Example
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        TextView tV1;
        NavigationView navigationView;
        DrawerLayout drawer;
        ISharedPreferences pref;
        bool stop = false;
        string result2;
        V7ToolBar toolbar;
        string result;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            pref = Application.GetSharedPreferences("backgroundColor_textColor", FileCreationMode.Private);
            result = pref.GetString("background", "White");
            result2 = pref.GetString("textColor", "Black");

            toolbar = FindViewById<V7ToolBar>(Resource.Id.toolbar);
            drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            tV1 = FindViewById<TextView>(Resource.Id.main_text);
        
            toolbar.SetTitle(Resource.String.title_name);
            toolbar.SetTitleTextColor(Color.White);

            SetSupportActionBar(toolbar);
           
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            //navigationView.ItemTextColor = ColorStateList.ValueOf(Color.ParseColor(result2));
            drawer.SetBackgroundColor(Color.ParseColor(result));
            toolbar.SetBackgroundColor(Color.ParseColor(result2));
            navigationView.SetBackgroundColor(Color.ParseColor(result2));
            Task.Run(async () => await Update());

        }

        private async Task Update()
        {
            while(!stop)
            {
                await Task.Delay(400);
                result = pref.GetString("background", "White");
                result2 = pref.GetString("textColor", "Black");
                RunOnUiThread(()=>
                {
                    tV1.SetTextColor(Color.ParseColor(result2));
                    drawer.SetBackgroundColor(Color.ParseColor(result));
                    toolbar.SetBackgroundColor(Color.Black);
                    navigationView.SetBackgroundColor(Color.ParseColor(result));
                    navigationView.ItemTextColor = ColorStateList.ValueOf(Color.ParseColor(result2));
                });
            }
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.nav_manage:
                    Intent intent = new Intent(this, typeof(Settings_Activity));
                    StartActivity(intent);
                    //tV1.Text = "В скором времени здесь появится окно с настройками";
                    break;
                case Resource.Id.dictionary:
                    tV1.Text = "В скором времени здесь появится окно с треньками";
                    break;
            }
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
    }
}

