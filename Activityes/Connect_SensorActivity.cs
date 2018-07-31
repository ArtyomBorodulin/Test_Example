using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Threading.Tasks;
using Android.Content;
using Android;
using Test_Example;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Support.Design;
using Android.Support.Design.Widget;
using Android.Graphics;
using V7ToolBar = Android.Support.V7.Widget.Toolbar;
using Test_Example.Change_color;
using Android.Support.V7.App;
using Android.Views;

namespace Test_Example.Activityes
{
    [Activity(Label = "Connect_SensorActivity",Theme ="@style/Theme.AppCompat.Light.NoActionBar")]
    public class Connect_SensorActivity : AppCompatActivity
    {
        private IHeartRateEnumerator _hrEnumerator;
        ISharedPreferences pref;
        private bool stop = false;
        IHeartRate _heartRate;
        ISharedPreferences colors;
        V7ToolBar toolBar;
        string background, textColor;
        private ListView _listDevices;
        private string selected = "";
        RelativeLayout relLayout;
        private TextView textView1;
        string[] permissions = new string[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };
        private List<string> listAdapter;
        List<string> favorites_sensors;
        FloatingActionButton fButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Connect_sensor);

            //Create a color;
            colors = GetSharedPreferences("backgroundColor_textColor", FileCreationMode.Private);
            background = colors.GetString("background", "White");
            textColor = colors.GetString("textColor", "Black");

            relLayout = FindViewById<RelativeLayout>(Resource.Id.rel);
            Toast.MakeText(this, $"{background},{textColor}", ToastLength.Short).Show();
            textView1 = FindViewById<TextView>(Resource.Id.t);
            fButton = FindViewById<FloatingActionButton>(Resource.Id.FloatingButton);
            favorites_sensors = new List<string>();
            _listDevices = FindViewById<ListView>(Resource.Id.ScanDevicesListView);
            fButton.SetColorFilter(Color.White);
            toolBar = FindViewById<V7ToolBar>(Resource.Id.toolbar);


            //Events
            _listDevices.ItemClick += ListView_ItemClick;
            fButton.Click += FButton_Click;

            SetSupportActionBar(toolBar);
            SetTitle(Resource.String.title2);
            listAdapter = new List<string>();
            MyListView1 myList1 = new MyListView1(this, listAdapter,Color.ParseColor(textColor));
            _listDevices.Adapter = myList1;
            textView1.SetTextColor(Color.ParseColor(textColor));
            fButton.SetColorFilter(Color.ParseColor(background));
            fButton.SetBackgroundColor(Color.ParseColor(textColor));
            relLayout.SetBackgroundColor(Color.ParseColor(background));



        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string selected = _listDevices.GetItemAtPosition(e.Position).ToString();
            favorites_sensors.Add(selected);
            pref = GetSharedPreferences("sensors", FileCreationMode.Private);
            var edit = pref.Edit();
            edit.PutStringSet("sensor", favorites_sensors);
            edit.Commit();
            Toast.MakeText(this, $"Устройство {selected} добавлено в 'Мои датчики'", ToastLength.Short).Show();
            Stoper(sender, e);

        }

        private void Stoper(object sender, EventArgs e)
        {
            if (_hrEnumerator != null)
            {
                _hrEnumerator.DeviceScanUpdate -= _hrEnumerator_DeviceScanUpdate;
                _hrEnumerator.DeviceScanTimeout -= _hrEnumerator_DeviceScanTimeout;
                _hrEnumerator.StopDeviceScan();
                _hrEnumerator = null;
                Toast.MakeText(this, "Сканирование завершено", ToastLength.Short).Show();
            }

        }

        private void FButton_Click(object sender, System.EventArgs e)
        {

            if (_hrEnumerator == null)
            {
                listAdapter.Clear();
                MyListView1 listView1 = new MyListView1(this, listAdapter,Color.White);
                _listDevices.Adapter = listView1;
                _hrEnumerator = new HeartRateEnumeratorForAndroid();
                _hrEnumerator.DeviceScanUpdate += _hrEnumerator_DeviceScanUpdate;
                _hrEnumerator.DeviceScanTimeout += _hrEnumerator_DeviceScanTimeout;
                _hrEnumerator.StartDeviceScan();

                Toast.MakeText(this, "Сканирование началось", ToastLength.Short).Show();
            }
        }

        private void _hrEnumerator_DeviceScanTimeout(object sender, EventArgs e)
        {
            Stoper(sender, e);
        }

        private void _hrEnumerator_DeviceScanUpdate(object sender, string e)
        {
            listAdapter.Add(e);
            MyListView1 myList1 = new MyListView1(this, listAdapter,Color.White);
            _listDevices.Adapter = myList1;

        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.My_Sensors)
            {
                Intent intent = new Intent(this, typeof(My_sensors));
                StartActivity(intent);
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }
        public override bool OnCreateOptionsMenu(IMenu menu)  // Когда вызван toolbar, что необходимо показывать
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.toolbar_values, menu);
            return true;
        }
    }
}