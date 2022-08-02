using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


static class Memory
{
    private static Process targetProcess;
    private static IntPtr targetProcessHandle;
    private const int ProcessVmRead = 0x0010;
    private const int ProcessVmWrite = 0x0020;
    private const int ProcessVmOperation = 0x0008;
    internal static ulong moduleBaseAddress;

    public static IntPtr OpenTargetProcessByPid(int pID, string moduleName = "")
    {
        targetProcess = Process.GetProcessById(pID);

        if (targetProcess != null)
        {
            targetProcessHandle = NativeMethods.OpenProcess(ProcessVmRead | ProcessVmOperation | ProcessVmWrite, false, targetProcess.Id);
            return ImageBaseAddress(moduleName);
        }

        return IntPtr.Zero;
    }

    private static bool HasActiveHandle()
    {
        if (targetProcessHandle != IntPtr.Zero)
        {
            return true;
        }

        return false;
    }

    private static IntPtr ImageBaseAddress(string dllname = "")
    {
        if (!HasActiveHandle())
        {
            return IntPtr.Zero;
        }

        var baseAddress = targetProcess.MainModule.BaseAddress;

        if (dllname != "")
        {
            var modules = targetProcess.Modules;

            foreach (ProcessModule procmodule in modules)
            {
                if (dllname == procmodule.ModuleName)
                {
                    baseAddress = procmodule.BaseAddress;
                }
            }
        }
        return baseAddress;

    }



    public static void WriteBytes(long address, byte[] buffer)
    {
        try
        {
            var size = buffer.Length;
            int bytesWritten = 0;

            NativeMethods.WriteProcessMemory((int)targetProcessHandle, address, buffer, size, ref bytesWritten);
        }
        catch
        {
        }
    }


    public static T Read<T>(long address)
    {
        try
        {
            var size = Marshal.SizeOf(typeof(T));
            var buffer = new byte[size];
            var read = 0;

            NativeMethods.ReadProcessMemory((int)targetProcessHandle, address, buffer, size, ref read);

            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var data = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));

            handle.Free();
            return data;
        }
        catch (Exception)
        {
            return default(T);
        }
    }

    public static byte[] ReadBytes(long address, int bufferSize)
    {
        if (!HasActiveHandle())
        {
            return new byte[0];
        }
        var bytesRead = 0;
        var buffer = new byte[bufferSize];
        NativeMethods.ReadProcessMemory((int)targetProcessHandle, address, buffer, buffer.Length, ref bytesRead);

        return buffer;
    }
}

