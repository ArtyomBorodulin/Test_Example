using System;
using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android;
using Android.Widget;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;


//Данный скрипт отвечает за поиск устройств. Содержимое данного класса:
//1. переменная _adapter - наш bluetooth адаптер на телефоне

namespace Test_Example.Activityes
{
    public class HeartRateEnumeratorForAndroid : IHeartRateEnumerator
    {
        private Plugin.BLE.Abstractions.Contracts.IAdapter _adapter;
        private CancellationTokenSource _cancellationTokenSource;
        public static List<IDevice> Devices;

        public event EventHandler<string> DeviceScanUpdate;
        public event EventHandler DeviceScanTimeout;

        public string DeviceName => throw new NotImplementedException();

        public bool IsScanning => throw new NotImplementedException();

        public HeartRateData GetCurrentHeartRateValue()
        {
            throw new NotImplementedException();
        }

        public bool StartDeviceScan()
        {
            _adapter = CrossBluetoothLE.Current.Adapter;
            Devices = new List<IDevice>();
            _cancellationTokenSource = new CancellationTokenSource();

            //Events
            _adapter.DeviceDiscovered += _adapter_DeviceDiscovered; // В случае, если было обнаружено новое устройство
            _adapter.ScanTimeoutElapsed += _adapter_ScanTimeoutElapsed;
            _adapter.ScanTimeout = 15000; // Сколько будет происходить сканирование bluetooth-устройств 15 секунд
            foreach (var device in _adapter.ConnectedDevices)
            {
                _adapter_DeviceDiscovered(device);
            }
            _adapter.StartScanningForDevicesAsync(serviceUuids: null, deviceFilter: null, cancellationToken: _cancellationTokenSource.Token); // Метод вызывает сканирование девайсов
            return true;
        }
        private void _adapter_DeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            _adapter_DeviceDiscovered(e.Device);
        }

        private void _adapter_DeviceDiscovered(IDevice device)
        {
            Devices?.Add(device);
            DeviceScanUpdate?.Invoke(this, device.Name);
        }

        private void _adapter_ScanTimeoutElapsed(object sender, EventArgs e)  // Событие автоматически вызывается, если было превышено допустимое время (в нашем случае - это 15 секунд)
        {
            Toast.MakeText(Application.Context, "Поиск завершён", ToastLength.Short).Show();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
            //DeviceScanUpdate?.Invoke(this, null);
        }

        public static IDevice SearchDeviceByName()
        {
            foreach (var device in Devices)
            {
                if (device.Name == "RHYTM+16622")
                    return device;
            }
            return null;
        }
        public bool StopDeviceScan()
        {
            _cancellationTokenSource.Cancel(true);
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
            _adapter.DeviceDiscovered -= _adapter_DeviceDiscovered;
            _adapter.ScanTimeoutElapsed -= _adapter_ScanTimeoutElapsed;
            return true;
        }

        public IHeartRate GetHeartRate(string name)
        {
            var hr = new HeartRateAndroidBLE(name);
            return hr;
        }
    }
}