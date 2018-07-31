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
using Plugin.BLE;
using Android.Bluetooth;

namespace Test_Example.Hood.Bluetooth
{
    class Controller
    {
        protected Context _context;
        protected BluetoothManager _manager;
        private BluetoothAdapter _adapter;
        public Controller(Context context)
        {
            _context = context;
        }

        public void StartScanBluetooth()
        {
            _manager = (BluetoothManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.BluetoothService);
            _adapter = _manager.Adapter;
            if(_adapter == null)
            {
                Toast.MakeText(_context, "Невозможно произвести сканирование, так как Bluetooth не поддерживается на вашем устройстве"
                    , ToastLength.Long).Show();
            }
            else
            {
                bool enabled = _adapter.IsEnabled;
                if(!enabled)
                {
                    _adapter.Enable();
                }
            }
        }
    }
}