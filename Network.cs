using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleWifi;

namespace IIPU_Networks
{
    class Network
    {
        public string Name { get; set; }
        public string SignalStrength { get; set; }
        public string Description { get; set; }
        public List<string> MAC { get; set; }
        public bool IsSecured { get; set; }
        public bool IsConnected { get; set; }

        public Network(string name, string signalStrength, string description, List<string> MACAddress, bool isSecured, bool isConnected)
        {
            Name = name;
            SignalStrength = signalStrength;
            Description = description;
            MAC = MACAddress;
            IsSecured = isSecured;
            IsConnected = isConnected;
        }

        public bool Connect(string password)
        {
            Wifi wifi = new Wifi();
            AccessPoint accessPoint = wifi.GetAccessPoints().FirstOrDefault(x => x.Name.Equals(Name));
            if (accessPoint != null)
            {
                AuthRequest authRequest = new AuthRequest(accessPoint);
                authRequest.Password = password;
                return accessPoint.Connect(authRequest);
            }
            return false;
        }

        public string GetMAC()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("MAC: ");
            foreach (var symbol in MAC)
            {
                builder.Append(symbol + "\r\n");
            }
            return builder.ToString();
        }
    }
}
}
