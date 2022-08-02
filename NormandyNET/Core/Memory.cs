using NormandyNET.Helpers;
using NormandyNET.Settings;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NormandyNET.Core
{
    internal class Memory
    {
        internal static int RPMCount;
        internal static int WPMCount;
        internal static int RPMCountPrev;
        internal static int WPMCountPrev;
        internal static ulong moduleBaseAddress;
        internal static double medianAddress = 0;
        internal static byte[] writeBuffer;
        internal static int InvalidAddressCount;
        internal static bool stopOnError = false;
        internal static bool? shortPtrDetected;
        internal static List<ulong> validPtrList = new List<ulong>();
        internal static double percentDeviationMin = 0.96d;
        internal static bool SlowMode;

        internal static void ResetFields()
        {
            InvalidAddressCount = 0;
            medianAddress = 0;
            shortPtrDetected = null;
            validPtrList.Clear();
            SlowMode = false;
        }

        internal static bool DetectShortPtr(ulong address)
        {
            if (address.ToString("x2").Length >= 7 && address.ToString("x2").Length <= 9)
            {

                return true;
            }
            else
            {

                return false;
            }
        }

        internal static bool IsValidPointer(ulong address)
        {
            if (address >= 0x1000000 && address < 0x7FFFFFFFFFF)

            {
                return true;
            }

            return false;

            if (shortPtrDetected == null)
            {
                shortPtrDetected = DetectShortPtr(address);
            }

            var is4ByteAligned = address % 4 == 0;
            var is8ByteAligned = address % 8 == 0;

            if (shortPtrDetected == true)
            {
                var addressStrLength = address.ToString("x2").Length;

                if (address >= 0x1000000 && address < 0x7FFFFFFFFFF)

                {
                    return true;
                }

                return false;
            }

            if (shortPtrDetected == false)
            {
                if (address.ToString("x2").Length != 11)
                {
                    return false;
                }

                if (address.ToString("x2").Contains("ffff"))
                {
                    return false;
                }

                if (address.ToString("x2").Contains("00"))
                {
                    if (address.ToString("x2").Contains("00000"))
                    {
                        return false;
                    }

                    if (address.ToString("x2").Contains("00000000"))
                    {


                        return false;
                    }

                    if (address.ToString("x2").Contains("000000"))
                    {

                        return false;
                    }
                }

                if (address >= 0x10000000000 && address < 0x30000000000)
                {
                    if (validPtrList.Count < 150 && !validPtrList.Contains(address))
                    {
                        validPtrList.Add(address);

                        medianAddress = (double)MathHelper.Median(validPtrList);
                    }

                    var percentFound = 0d;

                    if ((double)address > medianAddress)
                    {
                        percentFound = (double)medianAddress / address;
                    }
                    else
                    {
                        percentFound = (double)address / medianAddress;
                    }

                    if (percentFound > percentDeviationMin)
                    {

                        return true;
                    }

                    return false;
                }

                return false;
            }

            return false;
        }

        public static byte[] ReadBytes(ulong address, int size)
        {

            var buffer = new byte[size];

            if (DebugClass.UseUserModeServer)
            {
                SynchronousSocketClient.MemoryOperation(address, false, ref buffer);
            }
            else
            {
                SynchronousSocketDriverClient.MemoryOperation(address, false, ref buffer);
            }

            RPMCount++;
            return buffer;
        }

        public static T Read<T>(ulong address, bool reportValidation = true, bool validatePointer = true)
        {
            try
            {
                var size = Marshal.SizeOf(typeof(T));
                var buffer = new byte[size];


                buffer = ReadBytes(address, size);
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

        public static void WriteBytes(ulong address, ref byte[] buffer)
        {

            if (DebugClass.UseUserModeServer)
            {
                SynchronousSocketClient.MemoryOperation(address, true, ref buffer);
            }
            else
            {
                SynchronousSocketDriverClient.MemoryOperation(address, true, ref buffer);
            }

            WPMCount++;
            return;
        }

        public static void Write<T>(ulong address, T value)
        {
            try
            {
                var size = Marshal.SizeOf(typeof(T));
                var buffer = new byte[size];
                var gchandle = GCHandle.Alloc(value, GCHandleType.Pinned);
                Marshal.Copy(gchandle.AddrOfPinnedObject(), buffer, 0, size);
                gchandle.Free();

                WriteBytes(address, ref buffer);
            }
            catch
            {
            }
        }

        public static T ReadChain<T>(ulong address, uint[] offsets, bool reportValidation = true)
        {
            try
            {
                var size = Marshal.SizeOf(typeof(T));
                var buffer = new byte[size];


                for (int i = 0; i < offsets.Length - 1; i++)
                {

                    address += offsets[i];
                    address = Read<ulong>(address);

                    if (IsValidPointer(address) == false)
                    {
                        return default(T);
                    }
                }

                buffer = ReadBytes(address + offsets[offsets.Length - 1], size);
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
    }
}