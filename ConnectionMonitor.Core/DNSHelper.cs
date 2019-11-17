using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionMonitor.Core
{
    public class DNSHelper
    {
        public static readonly DNSHelper Instance = new DNSHelper();

        private Dictionary<string, IPHostEntry> _hostNames;

        private DNSHelper()
        {
            _hostNames = new Dictionary<string, IPHostEntry>();
        }

        public IPHostEntry ResolveHostEntry(IPAddress ip)
        {
            lock(_hostNames)
            {
                string strIP = ip.ToString();
                if (_hostNames.ContainsKey(strIP))
                    return _hostNames[strIP];

                try
                {
                    IPHostEntry result =  Dns.GetHostEntry(ip);
                    _hostNames[strIP] = result;
                }
                catch (Exception)
                {
                    _hostNames[strIP] = new IPHostEntry() { HostName = strIP };
                }

                return _hostNames[strIP];
            }
        }

        public IPHostEntry ResolveHostEntry(string strIPOrName)
        {
            lock(_hostNames)
            {
                strIPOrName = strIPOrName.ToUpper();
                if (_hostNames.ContainsKey(strIPOrName))
                    return _hostNames[strIPOrName];

                try
                {
                    IPHostEntry result = Dns.GetHostEntry(strIPOrName);
                    _hostNames[strIPOrName] = result;
                }
                catch (Exception)
                {
                    _hostNames[strIPOrName] = new IPHostEntry() { HostName = strIPOrName };
                }

                return _hostNames[strIPOrName];
            }
        }

    }
}
