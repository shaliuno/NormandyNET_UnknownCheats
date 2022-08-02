using NormandyNET.Core;
using System;

namespace NormandyNET.Helpers
{
    internal class SigScanner
    {
        private ulong startAddress;
        private ulong offsettedAddress = 0;
        private uint offset = 0;

        private byte[] dumpedRegion;
        private int querySize = 3072;
        private int imageSize = 25 * 1024 * 1024;

        internal SigScanner(ulong _startAddress)
        {
            startAddress = _startAddress;
            offsettedAddress = startAddress;
            dumpedRegion = new byte[querySize];
        }

        internal void DoScan()
        {
            var startTime = CommonHelpers.dateTimeHolder;
            var found = false;
            ulong foundAddress = 0;
            var nopBuffer = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
            do
            {
                dumpedRegion = Memory.ReadBytes(offsettedAddress, querySize);

                offset += (uint)querySize;

                var res = FindPattern(new byte[] { 0x55, 0x48, 0x8B, 0xEC, 0x48, 0x81, 0xEC, 0xF0, 0x00, 0x00, 0x00, 0x48, 0x89, 0x75, 0xE8, 0x48, 0x89, 0x7D, 0xF0, 0x4C, 0x89, 0x75, 0xF8, 0x48, 0x89, 0x8D, 0x40, 0xFF, 0xFF, 0xFF }, "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", 0);

                if (res != 0)
                {
                    foundAddress = res;
                    found = true;
                    break;
                }
                offsettedAddress += (uint)querySize;
            } while ((DateTime.UtcNow - startTime).TotalMinutes < 30);

            if (found)
            {
                Memory.WriteBytes(foundAddress + 0x2D5, ref nopBuffer);
            }
            else
            {
                Console.WriteLine($"Nothing found due to scan or timeout.");
            }
        }

        public ulong FindPattern(byte[] btPattern, string strMask, int nOffset)
        {
            try
            {
                if (strMask.Length != btPattern.Length)
                    return 0;

                for (int x = 0; x < dumpedRegion.Length; x++)
                {
                    if (this.MaskCheck(x, btPattern, strMask))
                    {
                        return offsettedAddress + (uint)(x + nOffset);
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private bool MaskCheck(int nOffset, byte[] btPattern, string strMask)
        {
            for (int x = 0; x < btPattern.Length; x++)
            {
                if (strMask[x] == '?')
                    continue;

                if ((strMask[x] == 'x') && (btPattern[x] != this.dumpedRegion[nOffset + x]))
                    return false;
            }

            return true;
        }
    }
}