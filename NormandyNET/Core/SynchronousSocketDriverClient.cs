using NormandyNET;
using NormandyNET.Helpers;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;


internal class SynchronousSocketDriverClient
{
    internal static int MTU = 1440;

    internal static void InitClient(string _ipAddress, int _ipPort)
    {
    }

    internal unsafe static ulong GetModuleBase(uint _pid, string moduleName)
    {
        return 0;
    }

    internal static void ShutdownClient()
    {
    }

    internal static ulong ProcessOpen(uint pid, string moduleName)
    {
        return 0;
    }

    internal static void MemoryOperation(ulong address, bool write, ref byte[] buffer)
    {
    }

    internal unsafe static ulong FindPatternAddr(uint _pid, byte[] signature, string mask, ulong start = 0x01UL, ulong end = 0xFFFFFFFFFFFFFFFF)
    {
        return 0;
    }
}