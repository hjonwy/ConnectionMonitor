using ConnectionMonitor.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionManager.WinService
{
    public partial class ConMonitorService : ServiceBase
    {
        public ConMonitorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Utils.Log.Info("ConnectionMonitor service starting...");

                    ConMonitorManager.Instance.OnTCPConnectionOpened -= OnTCPConnectionOpened;
                    ConMonitorManager.Instance.OnTCPConnectionClosed -= OnTCPConnectionClosed;

                    ConMonitorManager.Instance.OnTCPConnectionOpened += OnTCPConnectionOpened;
                    ConMonitorManager.Instance.OnTCPConnectionClosed += OnTCPConnectionClosed;

                    ConMonitorManager.Instance.Start();
                    Utils.Log.Info("ConnectionMonitor service started.");
                }
                catch(Exception e)
                {
                    Utils.Log.ErrorFormat("Start ConnectionMonitor service failed due to {0}", e.Message);
                    Utils.Log.Error(e.StackTrace);
                }
            });
        }

        protected override void OnStop()
        {
            Utils.Log.Info("ConnectionMonitor service Stopping...");
            ConMonitorManager.Instance.Stop();
            Utils.Log.Info("ConnectionMonitor service Stopped");
        }

        private void OnTCPConnectionClosed(TCPConnectionArg arg)
        {
            TCPConnectionInfo ti = arg.ConnectionInfo;
            string strConn = string.Format("Close, Device:{4}, Src {0}:{1} -> Dest {2}:{3}", ti.SrcIP, ti.SrcPort,
                ti.DestIP, ti.DestPort, ti.DeviceId);

            Utils.Log.Debug(strConn);
        }

        private void OnTCPConnectionOpened(TCPConnectionArg arg)
        {
            TCPConnectionInfo ti = arg.ConnectionInfo;
            string strConn = string.Format("Open, Device:{4}, Src {0}:{1} -> Dest {2}:{3}", ti.SrcIP, ti.SrcPort,
                ti.DestIP, ti.DestPort, ti.DeviceId);

            Utils.Log.Info(strConn);
        }
    }
}
