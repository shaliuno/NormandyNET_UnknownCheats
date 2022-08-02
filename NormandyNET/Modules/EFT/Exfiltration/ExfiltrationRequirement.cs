using NormandyNET.Core;

namespace NormandyNET.Modules.EFT.Exfiltration
{
    public enum ERequirementState
    {
        None,
        Empty,
        TransferItem,
        WorldEvent,
        NotEmpty,
        HasItem,
        WearsItem,
        EmptyOrSize,
        SkillLevel,
        Reference,
        ScavCooperation,
        Train,
        Timer
    }

    internal class ExfiltrationRequirement
    {
        private ExfiltrationPoint exfiltrationPoint;
        private ulong address;
        internal ERequirementState requirement;

        public ExfiltrationRequirement(ExfiltrationPoint exfiltrationPoint, ulong address)
        {
            this.exfiltrationPoint = exfiltrationPoint;
            this.address = address;
            requirement = (ERequirementState)Memory.Read<int>(address + ModuleEFT.offsetsEFT.ExfiltrationRequirement_Requirement);
        }
    }
}