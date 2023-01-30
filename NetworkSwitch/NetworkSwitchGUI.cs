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

            // fetch adapters and add them to combo box
            adapters = net_adapters();
            adapters.ForEach(item =>
            {
                if (!item.Contains("Loopback"))
                {
                comboBox1.Items.Add(item.ToString());
                }

            });
           
            // init first selection
            comboBox1.SelectedIndex = 0;
            this.UpdateCheckBoxStateOnSelectAdapter();

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ToggleCheckBox();
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
            this.UpdateCheckboxText(isAdapterEnabled);
        }

        public void UpdateCheckboxText(bool boolean)
        {
            if (boolean)
            {
                checkBox1.Text = "Enabled";
            }
            else
            {
                checkBox1.Text = "Disabled";
            }
        }

        public void ToggleCheckBox()
        {
            bool inverted = !checkBox1.Checked;
            NetworkController.Switch(adapters[comboBox1.SelectedIndex], inverted);
            // checkBox1.Checked = inverted; // reverse state on success // IMPORTANT, DO NOT UNCOMMENT THIS, it triggers a recursive call to this method du to alternating checkbox behavior
            UpdateCheckboxText(inverted); ; // Update checkbox UI
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetworkController.ResetAndReenable(adapters[comboBox1.SelectedIndex]);
            this.UpdateCheckBoxStateOnSelectAdapter();
        }
    }
}
