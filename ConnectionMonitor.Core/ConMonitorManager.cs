using System;
using System.Collections.Generic;

namespace ConnectionMonitor.Core
{
    public class TCPConnectionArg : EventArgs
    {
        public TCPConnectionInfo ConnectionInfo { get; set; }
    }

    public delegate void DelegateTCPConnectionOpen(TCPConnectionArg arg);

    public delegate void DelegateTCPConnectionClose(TCPConnectionArg arg);

    public class ConMonitorManager
    {
        private List<MonitorWorker> _workers;

        public static readonly ConMonitorManager Instance = new ConMonitorManager();

        public event DelegateTCPConnectionOpen OnTCPConnectionOpened;

        public event DelegateTCPConnectionClose OnTCPConnectionClosed;

        private ConMonitorManager()
        {
            _workers = new List<MonitorWorker>();
        }

        public void Start()
        {
            //Utils.Log.Info("MonitorManager starts");

            _workers.Clear();

            var devices = SharpPcap.LibPcap.LibPcapLiveDeviceList.Instance;

            // If no devices were found print an error
            if (devices.Count < 1)
            {
                //Utils.Log.Info("No devices were found on this machine");
                return;
            }

            int deviceId = 1;
            foreach (SharpPcap.LibPcap.LibPcapLiveDevice dev in devices)
            {
                MonitorWorker worker = new MonitorWorker(dev);
                worker.DeviceId = deviceId++;
                worker.OnTCPConnectOpened += Worker_OnTCPConnectOpened;
                worker.OnTCPConnectClosed += Worker_OnTCPConnectClosed;
                _workers.Add(worker);
                worker.Start();
            }
        }

        private void Worker_OnTCPConnectClosed(TCPConnectionArg arg)
        {
            if (OnTCPConnectionClosed != null)
            {
                OnTCPConnectionClosed.BeginInvoke(arg, null, null);
            }
        }

        private void Worker_OnTCPConnectOpened(TCPConnectionArg arg)
        {
            if (OnTCPConnectionOpened != null)
            {
                OnTCPConnectionOpened.BeginInvoke(arg, null, null);
            }
        }

        public void Stop()
        {
            foreach (MonitorWorker worker in _workers)
            {
                worker.Stop();
            }

            _workers.Clear();
        }
    }
}
