using Microsoft.Win32;
using NormandyNET;
using NormandyNET.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Security
{
    internal static class FingerprintExtras
    {
        [DllImport("iphlpapi.dll", CharSet = CharSet.Auto)]
        private static extern int GetBestInterface(UInt32 destAddr, out UInt32 bestIfIndex);

        public static IPAddress GetGatewayForDestination(IPAddress destinationAddress)
        {
            UInt32 destaddr = BitConverter.ToUInt32(destinationAddress.GetAddressBytes(), 0);

            uint interfaceIndex;
            int result = GetBestInterface(destaddr, out interfaceIndex);
            if (result != 0)
                throw new Win32Exception(result);

            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                var niprops = ni.GetIPProperties();
                if (niprops == null)
                    continue;

                var gateway = niprops.GatewayAddresses?.FirstOrDefault()?.Address;
                if (gateway == null)
                    continue;

                if (ni.Supports(NetworkInterfaceComponent.IPv4))
                {
                    var v4props = niprops.GetIPv4Properties();
                    if (v4props == null)
                        continue;

                    if (v4props.Index == interfaceIndex)
                        return gateway;
                }

                if (ni.Supports(NetworkInterfaceComponent.IPv6))
                {
                    var v6props = niprops.GetIPv6Properties();
                    if (v6props == null)
                        continue;

                    if (v6props.Index == interfaceIndex)
                        return gateway;
                }
            }

            return null;
        }

        public static string GetGatewayMacForDestination(IPAddress destinationAddress)
        {
            UInt32 destaddr = BitConverter.ToUInt32(destinationAddress.GetAddressBytes(), 0);

            uint interfaceIndex;
            int result = GetBestInterface(destaddr, out interfaceIndex);
            if (result != 0)
                throw new Win32Exception(result);

            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                var niprops = ni.GetIPProperties();
                if (niprops == null)
                    continue;

                var gateway = niprops.GatewayAddresses?.FirstOrDefault()?.Address;
                if (gateway == null)
                    continue;

                if (ni.Supports(NetworkInterfaceComponent.IPv4))
                {
                    var v4props = niprops.GetIPv4Properties();
                    if (v4props == null)
                        continue;

                    PhysicalAddress mac = ni.GetPhysicalAddress();

                    if (v4props.Index == interfaceIndex)
                        return mac.ToString();
                }
            }

            return null;
        }

        internal static string ValueExtra()
        {
            var sid = GetSID_VMP();
            var macPair = GetGatewayMacForDestination(IPAddress.Parse(WebClientHelper.activeEndpoint));
            var crc32 = new CRC32();

            var arrayOfBytes = Encoding.ASCII.GetBytes($"{sid}{macPair}");
            var hash = CommonHelpers.ByteArrayToString(crc32.ComputeHash(arrayOfBytes));
            return hash.ToUpper();
        }

        internal static string ValueExtraClean()
        {
            var sid = GetSID_VMP();
            var macPair = GetGatewayMacForDestination(IPAddress.Parse(WebClientHelper.activeEndpoint));

            return $"{sid}_._{macPair}";
        }

        [System.Reflection.ObfuscationAttribute(Feature = "Virtualization", Exclude = false)]
        internal static string GetSID_VMP()
        {
            String SID = "";
            string currentUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();

            RegistryKey regDir = Registry.LocalMachine;

            using (RegistryKey regKey = regDir.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\SessionData", true))
            {
                if (regKey != null)
                {
                    string[] valueNames = regKey.GetSubKeyNames();
                    for (int i = 0; i < valueNames.Length; i++)
                    {
                        using (RegistryKey key = regKey.OpenSubKey(valueNames[i], true))
                        {
                            string[] names = key.GetValueNames();
                            for (int e = 0; e < names.Length; e++)
                            {
                                if (names[e] == "LoggedOnSAMUser")
                                {
                                    if (key.GetValue(names[e]).ToString() == currentUserName)
                                        SID = key.GetValue("LoggedOnUserSID").ToString();
                                }
                            }
                        }
                    }
                }
            }
            return SID;
        }

        [System.Reflection.ObfuscationAttribute(Feature = "Virtualization", Exclude = false)]
        internal static string GetMacAdapters_VMP()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            long nicBytesReceived = 0;
            string nicDescription = "";
            string nicMac = "";
            string gateway = "";

            foreach (NetworkInterface adapter in adapters)
            {
                if (adapter.OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceStatistics stats = adapter.GetIPStatistics();
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    if (properties.GatewayAddresses.Count > 0 && properties.GatewayAddresses[0].Address != null)
                    {
                        var bytesReceived = stats.BytesReceived;

                        if (nicBytesReceived < bytesReceived)
                        {
                            nicBytesReceived = bytesReceived;
                            nicDescription = adapter.Description;
                            PhysicalAddress mac = adapter.GetPhysicalAddress();
                            nicMac = mac.ToString();
                            gateway = properties.GatewayAddresses[0].Address.ToString();
                        }
                    }
                }
            }

            return getMacByIp(gateway);
        }

        internal static string getMacByIp(string ip)
        {
            var macIpPairs = GetAllMacAddressesAndIppairs_VMP();
            int index = macIpPairs.FindIndex(x => x.IpAddress == ip);
            if (index >= 0)
            {
                return macIpPairs[index].MacAddress.ToUpper();
            }
            else
            {
                return null;
            }
        }

        [System.Reflection.ObfuscationAttribute(Feature = "Virtualization", Exclude = false)]
        internal static List<MacIpPair> GetAllMacAddressesAndIppairs_VMP()
        {
            List<MacIpPair> mip = new List<MacIpPair>();
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "arp";
            pProcess.StartInfo.Arguments = "-a ";
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();
            string cmdOutput = pProcess.StandardOutput.ReadToEnd();
            string pattern = @"(?<ip>([0-9]{1,3}\.?){4})\s*(?<mac>([a-f0-9]{2}-?){6})";

            foreach (Match m in Regex.Matches(cmdOutput, pattern, RegexOptions.IgnoreCase))
            {
                mip.Add(new MacIpPair()
                {
                    MacAddress = m.Groups["mac"].Value,
                    IpAddress = m.Groups["ip"].Value
                });
            }

            return mip;
        }

        internal struct MacIpPair
        {
            public string MacAddress;
            public string IpAddress;
        }

        public class CRC32
        {
            private readonly uint[] ChecksumTable;
            private readonly uint Polynomial = 0xEDB88320;

            public CRC32()
            {
                ChecksumTable = new uint[0x100];

                for (uint index = 0; index < 0x100; ++index)
                {
                    uint item = index;
                    for (int bit = 0; bit < 8; ++bit)
                        item = ((item & 1) != 0) ? (Polynomial ^ (item >> 1)) : (item >> 1);
                    ChecksumTable[index] = item;
                }
            }

            public byte[] ComputeHash(Stream stream)
            {
                uint result = 0xFFFFFFFF;

                int current;
                while ((current = stream.ReadByte()) != -1)
                    result = ChecksumTable[(result & 0xFF) ^ (byte)current] ^ (result >> 8);

                byte[] hash = BitConverter.GetBytes(~result);
                Array.Reverse(hash);
                return hash;
            }

            public byte[] ComputeHash(byte[] data)
            {
                using (MemoryStream stream = new MemoryStream(data))
                    return ComputeHash(stream);
            }
        }
    }
}