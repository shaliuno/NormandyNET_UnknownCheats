using NormandyNET.Core;
using NormandyNET.Modules.EFT.Exfiltration;

namespace NormandyNET.Modules.EFT
{
    internal class LocalGameWorld
    {
        internal ulong address;
        internal ExfiltrationController exfiltrationController;

        internal LocalGameWorld(ulong address)
        {
            this.address = address;
        }

        internal void GetExfiltrationController()
        {
            if (exfiltrationController != null)
            {
                return;
            }

            if (Memory.IsValidPointer(address) == false)
            {
                return;
            }

            exfiltrationController = new ExfiltrationController(this);
        }

        internal void Cleanup()
        {
            exfiltrationController?.Cleanup();
        }
    }
}