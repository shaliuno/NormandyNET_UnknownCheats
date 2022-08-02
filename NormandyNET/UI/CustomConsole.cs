using System;
using System.IO;
using System.Runtime.InteropServices;

namespace NormandyNET.UI
{
    internal static class CustomConsole
    {
        [DllImport("kernel32.dll",
               EntryPoint = "AllocConsole",
               SetLastError = true,
               CharSet = CharSet.Auto,
               CallingConvention = CallingConvention.StdCall)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(string lpFileName
           , [MarshalAs(UnmanagedType.U4)] DesiredAccess dwDesiredAccess
           , [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode
           , uint lpSecurityAttributes
           , [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition
           , [MarshalAs(UnmanagedType.U4)] FileAttributes dwFlagsAndAttributes
           , uint hTemplateFile);

        [Flags]
        private enum DesiredAccess : uint
        {
            GenericRead = 0x80000000,
            GenericWrite = 0x40000000,
            GenericExecute = 0x20000000,
            GenericAll = 0x10000000
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetStdHandle(StdHandle nStdHandle, IntPtr hHandle);

        private enum StdHandle : int
        {
            Input = -10,
            Output = -11,
            Error = -12
        }

        [System.Reflection.ObfuscationAttribute(Feature = "Virtualization", Exclude = false)]
        internal static void CreateConsole_VMP()
        {
            if (AllocConsole())
            {
                var stdOutHandle = CreateFile("CONOUT$", DesiredAccess.GenericRead | DesiredAccess.GenericWrite, FileShare.ReadWrite, 0, FileMode.Open, FileAttributes.Normal, 0);

                if (stdOutHandle == new IntPtr(-1))
                {
                }

                if (!SetStdHandle(StdHandle.Output, stdOutHandle))
                {
                }

                var standardOutput = new StreamWriter(Console.OpenStandardOutput());
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);

                if (System.Console.In != StreamReader.Null)
                {
                    System.Console.OutputEncoding = System.Text.Encoding.UTF8;
                    Console.SetBufferSize(Int16.MaxValue / 4 - 1, Int16.MaxValue / 4 - 1);
                    System.Console.Write("\n.\n");
                }
            }
        }
    }
}