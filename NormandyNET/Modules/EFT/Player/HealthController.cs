using NormandyNET.Core;
using System;
using System.Collections.Generic;

namespace NormandyNET.Modules.EFT
{
    internal class HealthController
    {
        internal ulong address;
        private EntityPlayer entityPlayer;
        private Dictionary<EBodyPart, BodyPartState> bodypartsHealth = new Dictionary<EBodyPart, BodyPartState>();

        internal HealthController(EntityPlayer entityPlayer)
        {
            this.entityPlayer = entityPlayer;
            address = Memory.Read<ulong>(entityPlayer.playerAddress + ModuleEFT.offsetsEFT.Player_HealthController);
        }

        

        internal bool IsAlive()
        {
            var isAlive = Memory.Read<byte>(address + 0x84) == 1 ? true : false;
            return isAlive;
        }

        internal int GetHealthPecent()
        {
            if (!Memory.IsValidPointer(address))
            {
                return 0;
            }

            var bodyPartStateDictionary = Memory.Read<ulong>(address + 0x50);

            if (!Memory.IsValidPointer(bodyPartStateDictionary))
            {
                return 0;
            }

            var bodyPartStateDictionaryInstance = Memory.Read<ulong>(bodyPartStateDictionary + 0x18);

            if (!Memory.IsValidPointer(bodyPartStateDictionaryInstance))
            {
                return 0;
            }

            float maxHealth = 0, health = 0;

            for (uint i = 0; i <= 6; i++)
            {
                var bodyPartStateAddress = Memory.Read<ulong>(bodyPartStateDictionaryInstance + 0x30 + (i * 0x18));

                if (!Memory.IsValidPointer(bodyPartStateAddress))
                {
                    return 0;
                }

                if (!bodypartsHealth.ContainsKey((EBodyPart)i))
                {
                    bodypartsHealth.Add((EBodyPart)i, new BodyPartState(bodyPartStateAddress));
                }

                bodypartsHealth.TryGetValue((EBodyPart)i, out BodyPartState bodyPartState);

                bodyPartState.Health.GetHealth();

                var HealthValueClass = Memory.Read<ulong>(bodyPartStateAddress + 0x10);

                if (!Memory.IsValidPointer(HealthValueClass))
                {
                    return 0;
                }

                var current_health = Memory.Read<float>(HealthValueClass + 0x10);
                var max_health = Memory.Read<float>(HealthValueClass + 0x14);

                health += current_health;
                maxHealth += max_health;
            }

            return (int)Math.Round(health / maxHealth * 100);
        }

        internal class BodyPartState
        {
            internal bool IsDestroyed;
            internal HealthValue Health;
            internal ulong address;

            internal BodyPartState(ulong address)
            {
                this.address = address;
                Health = new HealthValue(this);
            }
        }

        internal class HealthValue
        {
            protected ValueStruct Value;
            internal ulong address;

            internal HealthValue(BodyPartState bodyPartState)
            {
                address = Memory.Read<ulong>(bodyPartState.address + 010);
            }

            internal void GetHealth()
            {
                Value = Memory.Read<ValueStruct>(address + 0x10);
            }
        }

        internal struct ValueStruct
        {
            internal float Current;

            internal float Maximum;

            internal float Minimum;

            internal float OverDamageReceivedMultiplier;

            internal float Normalized
            {
                get
                {
                    return this.Current / this.Maximum;
                }
            }

            internal bool AtMinimum
            {
                get
                {
                    return !(this.Current - this.Minimum).Positive();
                }
            }

            internal bool AtMaximum
            {
                get
                {
                    return !(this.Maximum - this.Current).Positive();
                }
            }
        }
    }
}