using NormandyNET.Core;
using NormandyNET.Settings;
using System;
using System.Collections.Generic;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal class FlyHack : SigScannerClass<FlyHack>
    {
        internal static DateTime FlyHackTimeLast;
        internal static int FlyHackTimeLastRateMs = 1000;
        internal byte[] movementContext_FreeFallTime_offset;
        private EntityPlayer entityPlayer;
        private DateTime TimeLast;
        private int TimeLastRateMs = 75;

        internal FlyHack(ImprovementsController improvementsController)
        {
            this.entityPlayer = improvementsController.entityPlayer;
            movementContext_FreeFallTime_offset = BitConverter.GetBytes(ModuleEFT.offsetsEFT.Player_MovementContext_FreeFallTime).Reverse();

            signatures = new List<SigscanSignature>
            {
                new SigscanSignature($"F2 0F 5A E8  F3 41 0F 11 AF {movementContext_FreeFallTime_offset[3]:x2} {movementContext_FreeFallTime_offset[2]:x2} 00 00", "xxxxxxxxxxxxx", 4),

                new SigscanSignature($"F2 0F 5A E8  F3 41 0F 11 AF {movementContext_FreeFallTime_offset[3]:x2} {movementContext_FreeFallTime_offset[2]:x2} 00 00", "xxxxxxxxxxxxx", 4),
            };
        }

        internal void Check()
        {
            if (DebugClass.UseUserModeServer)
            {
                return;
            }

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.FlyHack.Enabled)
            {
                ScanForSignatures();

                if (sigscanDone && !notWorking)
                {
                    Enable();
                }
            }
            else
            {
                if (notWorking)
                {
                    sigscanDone = false;
                    notWorking = false;
                    WipeSignaturesAddresses();
                }

                if (sigscanDone && !notWorking)
                {
                    Disable();
                }
            }
        }

        internal void GetFreeFall()
        {
            var value = Memory.Read<float>(entityPlayer.movementContext.address + ModuleEFT.offsetsEFT.Player_MovementContext_FreeFallTime);
            Console.WriteLine(value);
        }

        internal bool HasErrors()
        {
            if (SameGame())
            {
                if (GotSignaturesAddressesInSettings() == false)
                {
                    notWorking = true;
                    return true;
                }

                RestoreAddressesFromSettings();
                sigscanDone = true;
                return false;
            }
            else
            {
                bytePatchDone = false;
                sigscanDone = false;

                return false;
            }
        }

        internal void KeepAirstuck()
        {
            if (signatures[0].address != 0 && signatures[1].address != 0 && bytePatchDone)
            {
                if (CommonHelpers.dateTimeHolder > FlyHackTimeLast)
                {
                    SetFreeFall(0);

                    FlyHackTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(FlyHackTimeLastRateMs);
                }
            }
        }

        internal void ScanForSignatures()
        {
            if (DebugClass.UseUserModeServer)
            {
                return;
            }

            if (HasErrors())
            {
                return;
            }

            if (sigscanDone || notWorking)
            {
                return;
            }

            var offset = 0x400000000UL;
            var test = 0UL;
            var inReadableRegion = false;

            if (!GotSignaturesAddresses())
            {
                Console.WriteLine($"FlyHack: ScanForSignatures via drver = {!DebugClass.UseUserModeServer}");

                scanInprogress = true;
                Console.WriteLine($"    No signature addresses. Good.");

                ulong endAddress = entityPlayer.movementContext.address + 0xfffffffff;

                for (int i = 0; i < signatures.Count; i++)
                {
                    ulong startAddress;

                    if (i == 0)
                    {
                        startAddress = GameObjectManager.gameObjectManagerAddress;
                    }
                    else
                    {
                        startAddress = signatures[0].address + (uint)signatures[0].originalBytes.Length;
                    }

                    Console.WriteLine($"    try {i + 1} Looking signature {CommonHelpers.ByteArrayToString(signatures[i].signature, true)}");
                    Console.WriteLine($"    starting at {startAddress:x2}");
                    Console.WriteLine($"    ending at {endAddress:x2}");

                    var tempAddress = SynchronousSocketDriverClient.FindPatternAddr(ModuleEFT.radarForm.settingsRadar.Network.lastGameProcessID, signatures[i].signature, signatures[i].mask, startAddress, endAddress);

                    if (tempAddress != 0)
                    {
                        var sigBytes = Memory.ReadBytes(tempAddress + signatures[i].offset, 9);
                        Console.WriteLine($"    signature_{i}_address: {tempAddress:x2}");
                        Console.WriteLine($"    signature_{i}_bytes: {CommonHelpers.ByteArrayToString(sigBytes, true)}");
                        signatures[i].originalBytes = sigBytes;
                        signatures[i].address = tempAddress;
                    }
                    else
                    {
                        sigscanDone = true;
                        notWorking = true;
                        Console.WriteLine($"NOT FOUND. BREAKING. FROM SEARCH.");
                        Console.WriteLine($"sigscanDone {sigscanDone}");
                        Console.WriteLine($"notWorking {notWorking}");
                        return;
                    }
                }

                sigscanDone = true;
                scanInprogress = false;

                StoreSettings();
            }
        }

        internal void SetFreeFall(float value)
        {
            Memory.Write<float>(entityPlayer.movementContext.address + ModuleEFT.offsetsEFT.Player_MovementContext_FreeFallTime, value);
        }

        internal bool ShowOSDWarning(out string text)
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.FlyHack.Enabled && notWorking)
            {
                text = "FlyHack\nSomething wrong with FlyHack.\nTry restarting EFT and trying again.";
                return true;
            }

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.FlyHack.Enabled && scanInprogress)
            {
                text = "FlyHack\nPlease wait, scan in progress.";
                return true;
            }

            text = "";
            return false;
        }

        internal void StoreSettings()
        {
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.FlyHack.ProcessID = ModuleEFT.radarForm.settingsRadar.Network.lastGameProcessID;
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.FlyHack.ImageBase = Memory.moduleBaseAddress;
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.FlyHack.Signatures = signatures;
        }

        private static bool SameGame()
        {
            var samePid = ModuleEFT.settingsForm.settingsJson.MemoryWriting.FlyHack.ProcessID == ModuleEFT.radarForm.settingsRadar.Network.lastGameProcessID;

            var imageBase = ModuleEFT.settingsForm.settingsJson.MemoryWriting.FlyHack.ImageBase == Memory.moduleBaseAddress;

            return (samePid & imageBase);
        }

        private void Disable()
        {
            if (GotSignaturesAddresses())
            {
                if (bytePatchDone == true)
                {
                    Console.WriteLine($"\nFlyHackDisable");
                    Console.WriteLine($"    address 1 == {signatures[0].address:x2}. Good.");
                    Console.WriteLine($"    address 2 == {signatures[1].address:x2}. Good.");
                    var buffer = new byte[] { 0xF3, 0x41, 0x0F, 0x11, 0xAF, 0xF8, 0x01, 0x00, 0x00 };
                    Memory.WriteBytes(signatures[0].address + 4, ref buffer);
                    Memory.WriteBytes(signatures[1].address + 4, ref buffer);
                    Console.WriteLine($"    Bytepatch reverted.");
                    Console.WriteLine($"    FreeFallTime will be set by the game itself.");
                    bytePatchDone = false;
                }
            }
        }

        private void Enable()
        {
            if (GotSignaturesAddresses())
            {
                if (bytePatchDone == false)
                {
                    Console.WriteLine($"\nFlyHackEnable");
                    Console.WriteLine($"    address 1 == {signatures[0].address:x2}. Good.");
                    Console.WriteLine($"    address 2 == {signatures[1].address:x2}. Good.");

                    var buffer = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, };
                    Memory.WriteBytes(signatures[0].address + 4, ref buffer);
                    Memory.WriteBytes(signatures[1].address + 4, ref buffer);
                    Console.WriteLine($"    Bytepatch applied.");
                    Console.WriteLine($"    FreeFallTime will be set to 0.");
                    bytePatchDone = true;
                }

                KeepAirstuck();
            }
        }

        private bool GotSignaturesAddressesInSettings()
        {
            var zeroAddresses = false;
            for (int i = 0; i < ModuleEFT.settingsForm.settingsJson.MemoryWriting.FlyHack.Signatures.Count; i++)
            {
                if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.FlyHack.Signatures[i].address == 0)
                {
                    zeroAddresses = true;
                }
            }

            return !zeroAddresses;
        }

        private void RestoreAddressesFromSettings()
        {
            for (int i = 0; i < ModuleEFT.settingsForm.settingsJson.MemoryWriting.FlyHack.Signatures.Count; i++)
            {
                signatures[i].address = ModuleEFT.settingsForm.settingsJson.MemoryWriting.FlyHack.Signatures[i].address;
                signatures[i].originalBytes = ModuleEFT.settingsForm.settingsJson.MemoryWriting.FlyHack.Signatures[i].originalBytes;
            }
        }
    }
}