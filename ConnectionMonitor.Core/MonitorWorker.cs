using PacketDotNet;
using PacketDotNet.Connections;
using PacketDotNet.Utils;
using SharpPcap;
using SharpPcap.LibPcap;
using SharpPcap.WinPcap;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectionMonitor.Core
{
    internal class MonitorWorker
    {
        private ICaptureDevice _device;
        private TcpConnectionManager _tcpConnectionManager;

        public int DeviceId { get; set; }

        public event DelegateTCPConnectionOpen OnTCPConnectOpened;

        public event DelegateTCPConnectionClose OnTCPConnectClosed;

        internal MonitorWorker(ICaptureDevice device)
        {
            _device = device;
            _tcpConnectionManager = new TcpConnectionManager();
        }

        internal void Start()
        {
            _device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);

            // Open the device for capturing
            int readTimeoutMilliseconds = 1000;
            //if (_device is AirPcapDevice)
            //{
            //    // NOTE: AirPcap devices cannot disable local capture
            //    var airPcap = _device as AirPcapDevice;
            //    airPcap.Open(OpenFlags.DataTransferUdp, readTimeoutMilliseconds);
            //}
            //else 
            if (_device is WinPcapDevice)
            {
                var winPcap = _device as WinPcapDevice;
                winPcap.Open(OpenFlags.DataTransferUdp | OpenFlags.NoCaptureLocal, readTimeoutMilliseconds);
            }
            else if (_device is LibPcapLiveDevice)
            {
                var livePcapDevice = _device as LibPcapLiveDevice;
                livePcapDevice.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
            }
            else
            {
                throw new System.InvalidOperationException("unknown device type of " + _device.GetType().ToString());
            }

            _tcpConnectionManager.OnConnectionFound += OnConnectionFound;

            _device.StartCapture();
        }

        internal void Stop()
        {
            _device.StopCapture();
            _device.Close();
        }

        private void OnConnectionFound(TcpConnection con)
        {
            TCPConnectionInfo ti = new TCPConnectionInfo();
            ti.SrcIP = con.Flows[0].address;
            ti.SrcPort = con.Flows[0].port;
            ti.DestIP = con.Flows[1].address;
            ti.DestPort = con.Flows[1].port;

            ti.DeviceId = DeviceId.ToString();

            if (OnTCPConnectOpened != null)
            {
                TCPConnectionArg arg = new TCPConnectionArg();
                arg.ConnectionInfo = ti;

                OnTCPConnectOpened.BeginInvoke(arg, null, null);
            }

            // receive notifications when the connection is closed
            con.OnConnectionClosed += this.OnConnectionClosed;
        }

        private void OnConnectionClosed(PosixTimeval timeval, TcpConnection con, TcpPacket tcp, TcpConnection.CloseType closeType)
        {
            if(OnTCPConnectClosed != null)
            {
                TCPConnectionInfo ti = new TCPConnectionInfo();
                ti.SrcIP = con.Flows[0].address;
                ti.SrcPort = con.Flows[0].port;
                ti.DestIP = con.Flows[1].address;
                ti.DestPort = con.Flows[1].port;

                ti.DeviceId = DeviceId.ToString();

                TCPConnectionArg arg = new TCPConnectionArg();
                arg.ConnectionInfo = ti;

                OnTCPConnectClosed(arg);
            }
        }

        private void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType,
                                                         e.Packet.Data);

            try
            {
                var tcpPacket = packet.Extract<TcpPacket>();

                // only pass tcp packets to the tcpConnectionManager
                if (tcpPacket != null)
                {
                    _tcpConnectionManager.ProcessPacket(e.Packet.Timeval, tcpPacket);
                }
            }
            catch { }
        }
    }
}
