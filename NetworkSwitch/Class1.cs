
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetworkSwitch
{
    // Code obtained from: https://gist.github.com/tommyready/df468fb96667fea3862e387656198c58
    internal class NetworkController
    {

        public void Switch(String name, bool enabled)
        {
            string networkInterfaceName = "";

            try
            {
                networkInterfaceName = name; // Set Network Interface from Arguments
                Task task;

                if (enabled)
                {
                    task = Task.Factory.StartNew(() => EnableAdapter(networkInterfaceName));
                } else
                {
                    task = Task.Factory.StartNew(() => DisableAdapter(networkInterfaceName));
                }
                // TaskOne.Wait();
            }
            catch (Exception e)
            {
                // Log Error Message
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "NetworkAdaptersUtility";
                    if (e.GetType().IsAssignableFrom(typeof(System.IndexOutOfRangeException)))
                    {
                        eventLog.WriteEntry("No Network Interface Provided", EventLogEntryType.Error, 101, 1);
                    }
                    else
                    {
                        eventLog.WriteEntry(e.Message, EventLogEntryType.Error, 101, 1);
                    }
                }
            }
        }


        public void ResetAndReenable(String name)
        {
            string networkInterfaceName = "";

            try
            {
                networkInterfaceName = name; // Set Network Interface from Arguments

                Task TaskOne = Task.Factory.StartNew(() => DisableAdapter(networkInterfaceName));
                TaskOne.Wait();
                Task TaskTwo = Task.Factory.StartNew(() => EnableAdapter(networkInterfaceName));

            }
            catch (Exception e)
            {
                // Log Error Message
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "NetworkAdaptersUtility";
                    if (e.GetType().IsAssignableFrom(typeof(System.IndexOutOfRangeException)))
                    {
                        eventLog.WriteEntry("No Network Interface Provided", EventLogEntryType.Error, 101, 1);
                    }
                    else
                    {
                        eventLog.WriteEntry(e.Message, EventLogEntryType.Error, 101, 1);
                    }
                }
            }
        }



        static void EnableAdapter(string interfaceName)
        {
            ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface set interface \"" + interfaceName + "\" enable");
            Process p = new Process();
            p.StartInfo = psi;
            p.Start();
        }

        static void DisableAdapter(string interfaceName)
        {
            ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface set interface \"" + interfaceName + "\" disable");
            Process p = new Process();
            p.StartInfo = psi;
            p.Start();
        }
    }
}
