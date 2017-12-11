using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleWifi;

namespace IIPU_Networks
{
    internal class Network
    {
        public string Name { get; set; }
        public string SignalStrength { get; set; }
        public string Description { get; set; }
        public List<string> Mac { get; set; }
        public bool IsSecured { get; set; }
        public bool IsConnected { get; set; }

        public Network(string name, string signalStrength, string description, List<string> macAddress, bool isSecured,
            bool isConnected)
        {
            Name = name;
            SignalStrength = signalStrength;
            Description = description;
            Mac = macAddress;
            IsSecured = isSecured;
            IsConnected = isConnected;
        }

        public bool Connect(string password)
        {
            var wifi = new Wifi();
            var accessPoint = wifi.GetAccessPoints().FirstOrDefault(x => x.Name.Equals(Name));
            if (accessPoint != null)
            {
                var authRequest = new AuthRequest(accessPoint);
                authRequest.Password = password;
                return accessPoint.Connect(authRequest);
            }
            return false;
        }

        public void Disconnect()
        {
            var wifi = new Wifi();
            wifi.Disconnect();
        }

        public string GetMac()
        {
            var builder = new StringBuilder();
            builder.Append("MAC: ");
            foreach (var symbol in Mac)
            {
                builder.Append(symbol + "\r\n");
            }
            return builder.ToString();
        }
    }
}