using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IIPU_Networks
{
    public partial class NetworksView : Form
    {
        private readonly NetworksSearcher _searcher = new NetworksSearcher();
        private List<Network> _networks;
        private bool _connection;
        private Network _currentNetwork;

        public NetworksView()
        {
            InitializeComponent();
        }

        private void NetworkView_Load(object sender, EventArgs e)
        {
            RefreshNetworkList();
            Timer.Enabled = true;
        }

        private void RefreshNetworkList()
        {
            _connection = false;
            _networks = _searcher.GetNetworks();
            NetworkList.Items.Clear();
            foreach (var network in _networks)
            {
                var item = new ListViewItem(network.Name);
                if (network.IsConnected)
                {
                    _connection = true;
                    PasswordField.Enabled = false;
                    ConnectionButton.Enabled = false;
                    DisconnectButton.Enabled = true;
                    _currentNetwork = network;
                }
                item.SubItems.Add(network.SignalStrength);
                NetworkList.Items.Add(item);
            }
        }

        private void NetworkList_MouseClick(object sender, MouseEventArgs e)
        {
            _currentNetwork = _networks[NetworkList.SelectedItems[0].Index];
            NetworkInformation.Text = _currentNetwork.Description + _currentNetwork.GetMac();
            if (_connection)
            {
                ConnectionStatusL.Text = @"Connected";
            }
            else
            {
                ConnectionStatusL.Text = @"Available";
                PasswordField.Enabled = _currentNetwork.IsSecured;
                PasswordField.Clear();
            }
        }

        private void ConnectionButton_Click(object sender, EventArgs e)
        {
            if (PasswordField.Text.Length > 0)
            {
                if (_currentNetwork.Connect(PasswordField.Text))
                {
                    ConnectionStatusL.Text = @"Connected";
                    PasswordField.Enabled = false;
                    ConnectionButton.Enabled = false;
                    DisconnectButton.Enabled = true;
                }
                else
                {
                    ConnectionStatusL.Text = @"Error";
                }
            }
            _connection = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RefreshNetworkList();
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            _currentNetwork.Disconnect();
            _connection = false;
            DisconnectButton.Enabled = false;
            ConnectionButton.Enabled = true;
            PasswordField.Clear();
            PasswordField.Enabled = true;
            Timer.Stop();
            Timer.Start();
        }
    }
}