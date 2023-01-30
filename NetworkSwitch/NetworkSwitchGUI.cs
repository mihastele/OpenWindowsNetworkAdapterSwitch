using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkSwitch
{
    public partial class NetworkSwitchGUI : Form
    {

        List<String> adapters;
        public NetworkSwitchGUI()
        {
            InitializeComponent();
            adapters = net_adapters();
            adapters.ForEach(item => comboBox1.Items.Add(item.ToString()));
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        public List<String> net_adapters()
        {
            List<String> values = new List<String>();
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                values.Add(nic.Name);
            }
            return values;
        }
    }
}
