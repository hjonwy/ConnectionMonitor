using ConnectionMonitor.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionMonitor.FormTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnStartMonitor_Click(object sender, EventArgs e)
        {
            ConMonitorManager.Instance.OnTCPConnectionOpened -= OnTCPConnectionOpened;
            ConMonitorManager.Instance.OnTCPConnectionClosed -= OnTCPConnectionClosed;

            ConMonitorManager.Instance.OnTCPConnectionOpened += OnTCPConnectionOpened;
            ConMonitorManager.Instance.OnTCPConnectionClosed += OnTCPConnectionClosed;

            ConMonitorManager.Instance.Start();
        }

        private void BtnStopMonitor_Click(object sender, EventArgs e)
        {
            ConMonitorManager.Instance.Stop();
        }

        private void OnTCPConnectionClosed(TCPConnectionArg arg)
        {

        }

        private void OnTCPConnectionOpened(TCPConnectionArg arg)
        {
            if(this.IsHandleCreated)
            {
                BeginInvoke((MethodInvoker)(() =>
                {
                    TCPConnectionInfo ti = arg.ConnectionInfo;
                    string strConn = string.Format("Open, Device:{4}, Src {0}:{1} -> Dest {2}:{3}", ti.SrcIP, ti.SrcPort,
                        ti.DestIP, ti.DestPort, ti.DeviceId);

                    lvConnections.Items.Add(strConn);
                }));
            }
        }
    }
}
