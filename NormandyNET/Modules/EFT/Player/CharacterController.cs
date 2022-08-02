using NormandyNET.Core;
using System;
using UnityEngine;

namespace NormandyNET.Modules.EFT
{
    internal class CharacterController
    {
        internal ulong address;
        internal ulong stamina;
        private EntityPlayer entityPlayer;

        private DateTime VelocityTimeLast;
        private Vector3 PositionPrev;
        private int VelocityTimeLastRateMs = 1000;

        public CharacterController(EntityPlayer entityPlayer)
        {
            this.entityPlayer = entityPlayer;
            address = Memory.Read<ulong>(entityPlayer.playerAddress + 0x28);
        }

        internal Vector3 UpdateVelocity()
        {
            VelocityTimeLastRateMs = 1000;
            var distance = Vector3.Distance(PositionPrev, entityPlayer.Position);
            var velocity = (distance / VelocityTimeLastRateMs);
            var diff = entityPlayer.Position - PositionPrev;

            if (CommonHelpers.dateTimeHolder > VelocityTimeLast)
            {
                VelocityTimeLast = CommonHelpers.dateTimeHolder.AddMilliseconds(VelocityTimeLastRateMs);
                PositionPrev = entityPlayer.Position;
            }

            return diff * velocity * 10f;
        }

        internal Vector3 Velocity()
        {
            if (Memory.IsValidPointer(address))
            {
                return Memory.Read<Vector3>(address + 0x48);
            }

            return Vector3.zero;
        }
    }
}