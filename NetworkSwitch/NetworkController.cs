
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace NetworkSwitch
{
    // Code obtained from: https://gist.github.com/tommyready/df468fb96667fea3862e387656198c58
    internal class NetworkController
    {

        public static void Switch(String name, bool enabled)
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


        public static void ResetAndReenable(String name)
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

        public static void ShowInterfaceSummary()
        {

            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in interfaces)
            {
                Console.WriteLine("Name: {0}", adapter.Name);
                Console.WriteLine(adapter.Description);
                Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                Console.WriteLine("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                Console.WriteLine("  Operational status ...................... : {0}",
                    adapter.OperationalStatus);
                string versions = "";

                // Create a display string for the supported IP versions.
                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    versions = "IPv4";
                }
                if (adapter.Supports(NetworkInterfaceComponent.IPv6))
                {
                    if (versions.Length > 0)
                    {
                        versions += " ";
                    }
                    versions += "IPv6";
                }
                Console.WriteLine("  IP version .............................. : {0}", versions);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static NetworkInterface getNetworkInterface(String name)
        {
            NetworkInterface nt = NetworkInterface.GetAllNetworkInterfaces().Where(inName => name == inName.Name).First();
            // Console.WriteLine(nt.Name + " Was selected");
            return nt;
        }

        public static bool isNetworkAdapterEnabled(String name)
        {
            NetworkInterface nc = NetworkController.getNetworkInterface(name);
            return nc.OperationalStatus == OperationalStatus.Up;
        }
    }
}
