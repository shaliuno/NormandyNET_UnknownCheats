using System.Collections.Generic;

namespace NormandyNET.Modules.EFT.Improvements
{
    internal abstract class SigScannerClass<T>
    {
        internal static bool bytePatchDone = false;
        internal static bool sigscanDone = false;
        internal static bool notWorking = false;
        internal static bool scanInprogress = false;
        internal List<SigscanSignature> signatures;

        internal void WipeSignaturesAddresses()
        {
            for (int i = 0; i < signatures.Count; i++)
            {
                signatures[i].address = 0;
            }
        }

        internal bool GotSignaturesAddresses()
        {
            var zeroAddresses = false;
            for (int i = 0; i < signatures.Count; i++)
            {
                if (signatures[i].address == 0)
                {
                    zeroAddresses = true;
                }
            }

            return !zeroAddresses;
        }
    }
}