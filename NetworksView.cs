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
        private bool _connection = false;

        public NetworksView()
        {
            InitializeComponent();
        }

        private void NetworkView_Load(object sender, EventArgs e)
        {
            RefreshNetworkList();
            ConnectionButton.Enabled = false;
            PasswordField.Enabled = false;
            Timer.Enabled = true;
        }

        private void RefreshNetworkList()
        {
            _networks = _searcher.GetNetworks();
            NetworkList.Items.Clear();
            foreach (var network in _networks)
            {
                var item = new ListViewItem(network.Name);
                item.SubItems.Add(network.SignalStrength);
                NetworkList.Items.Add(item);
            }
        }

        private void NetworkList_MouseClick(object sender, MouseEventArgs e)
        {
            var network = _networks[NetworkList.SelectedItems[0].Index];
            NetworkInformation.Text = network.Description + network.GetMac();
            if (network.IsConnected)
            {
                ConnectionStatusL.Text = @"Connected";
                PasswordField.Enabled = false;
                PasswordField.Clear();
                ConnectionButton.Enabled = false;
            }
            else
            {
                ConnectionStatusL.Text = @"Available";
                PasswordField.Enabled = network.IsSecured;
                ConnectionButton.Enabled = true;
            }
        }

        private void ConnectionButton_Click(object sender, EventArgs e)
        {
            if (PasswordField.Text.Length > 0)
            {
                if (_networks[NetworkList.SelectedItems[0].Index].Connect(PasswordField.Text))
                {
                    ConnectionStatusL.Text = @"Connected";
                    PasswordField.Enabled = false;
                    ConnectionButton.Enabled = false;
                    NetworkList.SelectedItems[0].Selected = false;
                }
                else
                {
                    ConnectionStatusL.Text = @"Error";
                }
            }
            _connection = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!_connection)
            {
                RefreshNetworkList();
            }
        }

        private void PasswordField_Click(object sender, EventArgs e)
        {
            _connection = true;
        }
    }
}