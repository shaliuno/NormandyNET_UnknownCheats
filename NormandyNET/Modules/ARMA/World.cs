using NormandyNET.Core;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace NormandyNET.Modules.ARMA
{
    internal class World
    {
        internal static Camera camera;
        internal static ulong world;
        internal static ulong playerOn;

        internal static TableClass EntityTable_Near = new TableClass(75);
        internal static TableClass EntityTable_Far = new TableClass(1500);
        internal static TableClass EntityTable_FarFar = new TableClass(1500);
        internal static TableClass EntityTable_FarFarFar = new TableClass(5000);
        internal static TableClass EntityTable_BuildingObjectsAndLoot = new TableClass(10000);
        internal static TableClass EntityTable_BuildingObjectsAndLoot_2 = new TableClass(10000);
        internal static TableClass EntityTable_Bullet = new TableClass(1);

        private DateTime PlayersInfoTime = CommonHelpers.dateTimeHolder;

        public class TableClass
        {
            internal ulong tableAddress;
            internal int tableSize;
            internal DateTime lastUpdated;
            internal int updateRateMsec;

            public TableClass(int _updateRateMsec)
            {
                updateRateMsec = _updateRateMsec;
                lastUpdated = CommonHelpers.dateTimeHolder;
            }

            internal void ParseDataBuffers(byte[] databuffer)
            {
                tableAddress = BitConverter.ToUInt64(databuffer, 0);
                tableSize = BitConverter.ToInt32(databuffer, 8);
            }

            internal void GetData(ulong address)
            {
                var databuffer = Memory.ReadBytes(address, sizeof(ulong) + sizeof(int));
                ParseDataBuffers(databuffer);
            }

            internal List<ulong> GetTableEntries(TableType tableType)
            {
                var entriesFresh = new List<ulong>();

                if (tableAddress == 0)
                {
                    return entriesFresh;
                }

                int offset = 0;
                int maxEntiresToRead = 16;
                int entiresToRead = maxEntiresToRead;

                if (entiresToRead > tableSize)
                {
                    entiresToRead = tableSize;
                }

                while (offset < tableSize)
                {
                    var databuffer = Memory.ReadBytes(tableAddress + (uint)offset * 0x8, sizeof(ulong) * entiresToRead);
                    
                    for (uint i = 0; i < entiresToRead; i++)
                    {
                        var entityPtr = tableAddress + ((uint)offset * 0x8);

                        var entityAddress = BitConverter.ToUInt64(databuffer, (int)(i * sizeof(ulong)));
                        
                        entriesFresh.Add(entityAddress);

                        offset++;
                    }

                    if ((tableSize - offset) < entiresToRead)
                    {
                        entiresToRead = tableSize - offset;
                    }
                }

                return entriesFresh;
            }
        }

        public World()
        {
        }

        internal static void GetInstance()
        {
                        world = Memory.Read<ulong>(Memory.moduleBaseAddress + ModuleARMA.offsetsARMA.World, false, false);
                    }

        internal static void GetCamera()
        {
            
            if (!Memory.IsValidPointer(world))
            {
                Camera.camera = 0;
                return;
            }

            Camera.camera = Memory.Read<ulong>(world + ModuleARMA.offsetsARMA.WorldCamera);
                    }

        internal static void GetLocalEntity()
        {
                        
            if (!Memory.IsValidPointer(world))
            {
                playerOn = 0;
                                return;
            }

            {
                playerOn = Memory.Read<ulong>(world + ModuleARMA.offsetsARMA.WorldPlayerOn);
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
                var sortedObject = Memory.Read<ulong>(playerOn + ModuleARMA.offsetsARMA.Entity_SortedObject);
                var visualState = Memory.Read<ulong>(sortedObject + ModuleARMA.offsetsARMA.Entity_FutureVisualState);

                if (!Memory.IsValidPointer(visualState))
                {
                    return Vector3.Zero;
                }
                else
                {
                    var databuffer = Memory.ReadBytes(visualState + ModuleARMA.offsetsARMA.VisualState_Direction, sizeof(float) * 6);

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

        internal static void CleanTables()
        {
            EntityTable_Near.tableAddress = 0;
            EntityTable_Far.tableAddress = 0;
            EntityTable_FarFar.tableAddress = 0;
            EntityTable_FarFarFar.tableAddress = 0;
            EntityTable_BuildingObjectsAndLoot.tableAddress = 0;
            EntityTable_BuildingObjectsAndLoot_2.tableAddress = 0;
            EntityTable_Bullet.tableAddress = 0;
        }

        internal static void GetEntityTables()
        {
            
            World.EntityTable_Near.GetData(World.world + ModuleARMA.offsetsARMA.EntityTable_Near);
            World.EntityTable_Far.GetData(World.world + ModuleARMA.offsetsARMA.EntityTable_Far);
            World.EntityTable_FarFar.GetData(World.world + ModuleARMA.offsetsARMA.EntityTable_FarFar);
            World.EntityTable_FarFarFar.GetData(World.world + ModuleARMA.offsetsARMA.EntityTable_FarFarFar);

            World.EntityTable_BuildingObjectsAndLoot.GetData(World.world + ModuleARMA.offsetsARMA.EntityTable_BuildingObjectsAndLoot);
            World.EntityTable_BuildingObjectsAndLoot_2.GetData(World.world + ModuleARMA.offsetsARMA.EntityTable_BuildingObjectsAndLoot_2);

                                                                                            }

        internal static List<EntityArma> GetBullets()
        {
            var databuffer = Memory.ReadBytes(World.world + ModuleARMA.offsetsARMA.EntityTable_Bullet, sizeof(ulong) + sizeof(int));
            World.EntityTable_Bullet.ParseDataBuffers(databuffer);

            
            var tableAddress = EntityTable_Bullet.tableAddress;
            var tableSize = World.EntityTable_Bullet.tableSize;
            var playersListNew = new List<EntityArma>();

            if (tableAddress == 0)
            {
                return playersListNew;
            }

            return playersListNew;
        }
    }
}