using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ConnectionMonitor.Core
{
    public class TCPConnectionInfo
    {
        public IPAddress SrcIP { get; set; }

        public string SrcHost { get; set; }

        public int SrcPort { get; set; }

        public IPAddress DestIP { get; set; }

        public string DestHost { get; set; }

        public int DestPort { get; set; }

        public string DeviceId { get; set; }
    }
}
