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
            // NetworkInterface nc = NetworkController.getNetworkInterface(adapters[comboBox1.SelectedIndex]);
            // checkBox1.Text = nc.OperationalStatus.ToString();
            this.UpdateCheckBoxStateOnSelectAdapter();
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

        public void UpdateCheckBoxStateOnSelectAdapter()
        {
            bool isAdapterEnabled = NetworkController.isNetworkAdapterEnabled(adapters[comboBox1.SelectedIndex]);
            if(isAdapterEnabled)
            {
                checkBox1.Text = "Enabled";
            } else
            {
                checkBox1.Text = "Disabled";
            }
        }

        public void ToggleCheckBox()
        {
            bool inverted = checkBox1.Checked;
            NetworkController.Switch(adapters[comboBox1.SelectedIndex], inverted);
            checkBox1.Checked = inverted; // reverse state on success
        }

        
    }
}
