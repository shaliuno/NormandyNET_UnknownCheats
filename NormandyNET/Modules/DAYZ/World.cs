using NormandyNET.Core;
using System;
using System.Numerics;

namespace NormandyNET.Modules.DAYZ
{
    internal class World
    {
        internal static Camera camera;

        internal static ulong world;
        internal static ulong playerOn;

        public World()
        {
        }

        internal static void GetInstance()
        {
                        world = Memory.Read<ulong>(Memory.moduleBaseAddress + ModuleDAYZ.offsetsDAYZ.World, false, false);

                    }

        internal static void GetCamera()
        {
            
            if (!Memory.IsValidPointer(world))
            {
                Camera.camera = 0;
                return;
            }

            Camera.camera = Memory.Read<ulong>(world + ModuleDAYZ.offsetsDAYZ.World_Camera);
                    }

        internal static void GetLocalEntity()
        {
                        
            if (!Memory.IsValidPointer(world))
            {
                playerOn = 0;
                                return;
            }

            {
                playerOn = Memory.Read<ulong>(world + ModuleDAYZ.offsetsDAYZ.World_PlayerOn);
                            }
        }

        internal static Vector3 GetLocalEntityPosition()
        {
            
            if (!Memory.IsValidPointer(playerOn))
            {
                return Vector3.Zero;
            }

            if (playerOn != 0)
            {
                var sortedObject = Memory.Read<ulong>(playerOn + ModuleDAYZ.offsetsDAYZ.EntitySortedObject);
                var visualState = Memory.Read<ulong>(sortedObject + ModuleDAYZ.offsetsDAYZ.FutureVisualState);

                if (!Memory.IsValidPointer(visualState))
                {
                    return Vector3.Zero;
                }
                else
                {
                    var databuffer = Memory.ReadBytes(visualState + ModuleDAYZ.offsetsDAYZ.VisualState_Direction, sizeof(float) * 6);

                    var Position = new Vector3(
                           BitConverter.ToSingle(databuffer, 0x0 + 0xC),
                           BitConverter.ToSingle(databuffer, 0x4 + 0xC),
                           BitConverter.ToSingle(databuffer, 0x8 + 0xC));

                    
                    return Position;
                }
            }
            else
            {
                return Vector3.Zero;
            }
        }
    }
}