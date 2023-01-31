using System.Diagnostics;
using System.Net.NetworkInformation;

namespace NetworkSwitch
{
    // Code obtained from: https://gist.github.com/tommyready/df468fb96667fea3862e387656198c58
    internal class NetworkController
    {
        public static void Switch(String name, bool enabled)
        {
            try
            {
                Task task;
                if (enabled)
                    task = Task.Factory.StartNew(() => EnableAdapter(name));
                else
                    task = Task.Factory.StartNew(() => DisableAdapter(name));
                task.Wait();
                // TaskOne.Wait();
            }
            catch (Exception e)
            {
                LogErrorMessage(e);
            }
        }

        public static void ResetAndReenable(String name)
        {
            try
            {
                var TaskOne = Task.Factory.StartNew(() => DisableAdapter(name));
                TaskOne.Wait();
                var TaskTwo = Task.Factory.StartNew(() => EnableAdapter(name));
            }
            catch (Exception e)
            {
                LogErrorMessage(e);
            }
        }

        static void EnableAdapter(string interfaceName)
        {
            var psi = new ProcessStartInfo("netsh", "interface set interface \"" + interfaceName + "\" enable");
            var p = new Process();
            p.StartInfo = psi;
            p.Start();
        }

        static void DisableAdapter(string interfaceName)
        {
            var psi = new ProcessStartInfo("netsh", "interface set interface \"" + interfaceName + "\" disable");
            var p = new Process();
            p.StartInfo = psi;
            p.Start();
        }

        public static void ShowInterfaceSummary()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var adapter in interfaces)
            {
                Console.WriteLine("Name: {0}", adapter.Name);
                Console.WriteLine(adapter.Description);
                Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                Console.WriteLine("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                Console.WriteLine("  Operational status ...................... : {0}", adapter.OperationalStatus);
                var versions = "";

                // Create a display string for the supported IP versions.
                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                    versions = "IPv4";
                if (adapter.Supports(NetworkInterfaceComponent.IPv6))
                {
                    if (versions.Length > 0)
                        versions += " ";
                    versions += "IPv6";
                }
                Console.WriteLine("  IP version .............................. : {0}", versions);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static NetworkInterface GetNetworkInterface(String name)
        {
            var nt = NetworkInterface.GetAllNetworkInterfaces().Where(inName => name == inName.Name).First();
            // Console.WriteLine(nt.Name + " Was selected");
            return nt;
        }

        public static bool IsNetworkAdapterEnabled(String name)
        {
            var nc = GetNetworkInterface(name);
            return nc.OperationalStatus == OperationalStatus.Up;
        }

        private static void LogErrorMessage(Exception e)
        {
            using (var eventLog = new EventLog("Application"))
            {
                eventLog.Source = "NetworkAdaptersUtility";
                eventLog.WriteEntry(
                    e is IndexOutOfRangeException ? "No Network Interface Provided" : e.Message,
                    EventLogEntryType.Error, 101, 1);
            }
        }
    }
}
