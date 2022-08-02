using NormandyNET.Core;
using System;
using UnityEngine;

namespace NormandyNET.Modules.EFT
{
    internal class MovementContext
    {
        internal ulong address;
        private EntityPlayer entityPlayer;
        private DateTime TimeLast;
        private int TimeLastRateMs = 750;

        public MovementContext(EntityPlayer entityPlayer)
        {
            this.entityPlayer = entityPlayer;
            address = Memory.Read<ulong>(entityPlayer.playerAddress + ModuleEFT.offsetsEFT.Player_MovementContext);
        }

        internal float GetTilt()
        {
            return Memory.Read<float>(address + ModuleEFT.offsetsEFT.Player_MovementContext_Tilt);
        }

        internal void WriteAngle(Vector2 pos)
        {
            if (!Memory.IsValidPointer(this.address))
            {
                return;
            }

            var floatArray = new float[] { pos.x, pos.y };
            byte[] byteArray = new byte[floatArray.Length * 4];
            Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);
            Memory.WriteBytes(address + ModuleEFT.offsetsEFT.Player_MovementContext_AzimuthAndPosition, ref byteArray);
        }

        [System.Reflection.ObfuscationAttribute(Feature = "Virtualization", Exclude = false)]
        internal void AdjustRotationClamp()
        {
            if (CommonHelpers.dateTimeHolder > TimeLast)
            {
                TimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(TimeLastRateMs);
            }

            var CurrentState = Memory.Read<ulong>(address + ModuleEFT.offsetsEFT.Player_MovementContext_MovementState_Current);

            var RotationSpeedClampAddr = CurrentState + ModuleEFT.offsetsEFT.Player_MovementState_RotationSpeedClamp;
            var StateSensitivityAddr = CurrentState + ModuleEFT.offsetsEFT.Player_MovementState_StateSensitivity;

            var RotationSpeedClamp = Memory.Read<float>(RotationSpeedClampAddr);
            var StateSensitivity = Memory.Read<float>(StateSensitivityAddr);

            if (RotationSpeedClamp != 99)
            {
                Memory.Write<float>(RotationSpeedClampAddr, 99f);
            }

            if (StateSensitivity != 1)
            {
                Memory.Write<float>(StateSensitivityAddr, 1f);
            }
        }
    }
}