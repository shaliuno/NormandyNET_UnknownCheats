using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public static class NativeMethods
{
    [DllImport("kernel32.dll")]
    internal static extern IntPtr OpenProcess(int desiredAccess, bool inheritHandle, int processId);

    [DllImport("kernel32.dll")]
    internal static extern bool CloseHandle(IntPtr handle);

    [DllImport("kernel32.dll")]
    internal static extern bool ReadProcessMemory(int processHandle, long processBaseAddress, byte[] biffer, int size, ref int numberOfBytesRead);

    [DllImport("kernel32.dll")]
    internal static extern bool WriteProcessMemory(int processHandle, long processBaseAddress, byte[] biffer, int size, ref int numberOfBytesWritten);
}