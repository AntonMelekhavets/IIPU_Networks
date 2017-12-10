using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleWifi;
using NativeWifi;

namespace IIPU_Networks
{
    internal class NetworksSearcher
    {
        private readonly Wifi _wifi;
        private readonly WlanClient _wlanClient;

        public NetworksSearcher()
        {
            _wifi = new Wifi();
            _wlanClient = new WlanClient();
        }

        public List<Network> GetNetworks()
        {
            var networks = new List<Network>();
            var accessPoints = _wifi.GetAccessPoints();
            foreach (var accessPoint in accessPoints)
            {
                networks.Add(new Network(accessPoint.Name,
                    accessPoint.SignalStrength.ToString() + "%",
                    accessPoint.ToString(),
                    GetMac(accessPoint),
                    accessPoint.IsSecure,
                    accessPoint.IsConnected)
                );
            }
            return networks;
        }

        private List<string> GetMac(AccessPoint accessPoint)
        {
            var wlanInterface = _wlanClient.Interfaces.FirstOrDefault();
            return wlanInterface?.GetNetworkBssList()
                .Where(x => Encoding.ASCII.GetString(x.dot11Ssid.SSID, 0, (int)x.dot11Ssid.SSIDLength).Equals(accessPoint.Name))
                .Select(y => MacToString(y)).ToList();
        }

        private string MacToString(Wlan.WlanBssEntry entry)
        {
            var macBuilder = new StringBuilder();
            foreach (byte mByte in entry.dot11Bssid)
            {
                macBuilder.Append(mByte.ToString("X"));
                macBuilder.Append("-");
            }
            macBuilder.Remove(macBuilder.Length - 1, 1);
            return macBuilder.ToString();
        }
    }
}
