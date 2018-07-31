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
namespace Test_Example.Activityes
{

    public class HeartRateData
    {
        public DateTime Timestamp;
        public int Value;
        public DateTime? TimestampBatteryLevel;
        public int? BatteryLevel;
    }
    public interface IHeartRate
    {
        string DeviceName { get; }
        bool Start();
        bool IsRunning { get; }
        void Stop();
        HeartRateData GetCurrentHeartRateValue();
    }

}