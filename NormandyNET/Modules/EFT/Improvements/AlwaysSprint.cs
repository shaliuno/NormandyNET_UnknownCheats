using Newtonsoft.Json;
using NormandyNET.Core;
using NormandyNET.Settings;
using System;
using System.Collections.Generic;

namespace NormandyNET.Modules.EFT.Improvements
{
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class SigscanSignature
    {
        public ulong address;

        public byte[] originalBytes;

        [NonSerialized]
        internal readonly string mask;

        [NonSerialized]
        internal readonly uint offset;

        [NonSerialized]
        internal readonly byte[] signature;

        [JsonConstructor]
        public SigscanSignature(byte[] signature, string mask, uint offset)
        {
            this.signature = signature;
            this.mask = mask;
            this.offset = offset;
        }

        public SigscanSignature(string signature, string mask, uint offset)
        {
            this.signature = CommonHelpers.StringToByteArray(signature);
            this.mask = mask;
            this.offset = offset;
        }
    }

    internal class AlwaysSprint : SigScannerClass<AlwaysSprint>
    {
        public int? flags;
        internal byte[] ePhysicalCondition_offset;
        private EntityPlayer entityPlayer;
        private DateTime ExtrasTimeLast;
        private DateTime TimeLast;
        private int TimeLastRateMs = 300;

        internal AlwaysSprint(ImprovementsController improvementsController)
        {
            this.entityPlayer = improvementsController.entityPlayer;
            ePhysicalCondition_offset = BitConverter.GetBytes(ModuleEFT.offsetsEFT.Player_MovementContext_EPhysicalCondition).Reverse();
            signatures = new List<SigscanSignature>
            {
                new SigscanSignature($"F7 D1 23 C1 41 89 87 {ePhysicalCondition_offset[3]:x2} {ePhysicalCondition_offset[2]:x2} 00 00 EB 0D", "xxxxxxxxxxxxx", 4),

                new SigscanSignature($"41 89 86 {ePhysicalCondition_offset[3]:x2} {ePhysicalCondition_offset[2]:x2} 00 00 EB 85 ", "xxxxxxxxx", 0),
             };
        }

        internal void Check()
        {
            if (DebugClass.UseUserModeServer)
            {
                return;
            }

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprintAltMode.Enabled)
            {
                return;
            }

            if (CommonHelpers.dateTimeHolder > TimeLast)
            {
                TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);
            }
            else
            {
                return;
            }

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprint.Enabled)
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

            Console.WriteLine($"sigscanDone {sigscanDone}");
            Console.WriteLine($"\n------------------------");

            var offset = 0x300000000UL;
            var test = 0UL;
            var inReadableRegion = false;

            if (!GotSignaturesAddresses())
            {
                Console.WriteLine($"AlwaysSprint: ScanForSignatures via drver = {!DebugClass.UseUserModeServer}");

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

        internal bool ShowOSDWarning(out string text)
        {
            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprint.Enabled && notWorking)
            {
                text = "AlwaysSprint\nSomething wrong with AlwaysSprint.\nTry restarting EFT and trying again.";
                return true;
            }

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprint.Enabled && scanInprogress)
            {
                text = "AlwaysSprint\nPlease wait, scan in progress.";
                return true;
            }

            text = "";
            return false;
        }

        internal void StoreSettings()
        {
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprint.ProcessID = ModuleEFT.radarForm.settingsRadar.Network.lastGameProcessID;
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprint.ImageBase = Memory.moduleBaseAddress;
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprint.Signatures = signatures;
        }

        private static bool SameGame()
        {
            var samePid = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprint.ProcessID == ModuleEFT.radarForm.settingsRadar.Network.lastGameProcessID;

            var imageBase = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprint.ImageBase == Memory.moduleBaseAddress;

            return (samePid & imageBase);
        }

        private void Disable()
        {
            if (GotSignaturesAddresses())
            {
                if (bytePatchDone == true)
                {
                    Console.WriteLine($"\nAlwaysSprint Disable");

                    for (int i = 0; i < signatures.Count; i++)
                    {
                        Console.WriteLine($"    address {i} == {signatures[i].address:x2}. Good.");

                        var buffer = signatures[i].originalBytes;
                        Memory.WriteBytes(signatures[i].address + signatures[i].offset, ref buffer);
                    }

                    Console.WriteLine($"    Bytepatch reverted.");
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
                    Console.WriteLine($"\nAlwaysSprint Enable");

                    var buffer = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };

                    for (int i = 0; i < signatures.Count; i++)
                    {
                        Console.WriteLine($"    address {i} == {signatures[i].address:x2}. Good.");
                        Memory.WriteBytes(signatures[i].address + signatures[i].offset, ref buffer);
                    }

                    Console.WriteLine($"    Bytepatch applied.");
                    bytePatchDone = true;
                }

                ResetPhysicalCondition();
            }
        }

        private void GetFlags()
        {
            flags = Memory.Read<int>(entityPlayer.movementContext.address + ModuleEFT.offsetsEFT.Player_MovementContext_EPhysicalCondition);
        }

        private bool GotSignaturesAddressesInSettings()
        {
            var zeroAddresses = false;
            for (int i = 0; i < ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprint.Signatures.Count; i++)
            {
                if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprint.Signatures[i].address == 0)
                {
                    zeroAddresses = true;
                }
            }

            return !zeroAddresses;
        }

        internal void ResetPhysicalCondition()
        {
            GetFlags();

            foreach (var condition in Enum.GetValues(typeof(EPhysicalCondition)))
            {
                var can = ((int)(EPhysicalCondition)condition & flags) == 0;
                if (!can)
                {
                    SetPlayerFlag((EPhysicalCondition)condition, true);
                }
            }
        }

        private void RestoreAddressesFromSettings()
        {
            for (int i = 0; i < ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprint.Signatures.Count; i++)
            {
                signatures[i].address = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprint.Signatures[i].address;
                signatures[i].originalBytes = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AlwaysSprint.Signatures[i].originalBytes;
            }
        }

        private void SetPlayerFlag(EPhysicalCondition flag, bool setFlag)
        {
            if (flags == null)
            {
                GetFlags();
            }

            if (flags != null)
            {
                int flagToWrite;

                if (!setFlag)
                {
                    flagToWrite = (int)flags | (int)flag;
                }
                else
                {
                    flagToWrite = (int)flags & ~(int)flag;
                }


                Memory.Write(entityPlayer.movementContext.address + ModuleEFT.offsetsEFT.Player_MovementContext_EPhysicalCondition, flagToWrite);
            }
        }
    }
}