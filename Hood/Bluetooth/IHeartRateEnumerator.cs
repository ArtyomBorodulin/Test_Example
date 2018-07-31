using System;
using System.Collections.Generic;

namespace Test_Example.Activityes
{

    public interface IHeartRateEnumerator
    {
        bool StartDeviceScan();
        bool StopDeviceScan();
        event EventHandler<string> DeviceScanUpdate;
        event EventHandler DeviceScanTimeout;

        IHeartRate GetHeartRate(string name);
    }

}