using NormandyNET.Core;
using NormandyNET.Modules.EFT.Objects;
using System.Collections.Generic;

namespace NormandyNET.Modules.EFT
{
    internal static class GameObjectManager
    {
        internal static ulong gameObjectManagerAddress;

        private static bool debugLog = false;

        private static uint tagged_object_ = 0x18;
        private static uint last_tagged_object_ = 0x10;
        private static uint active_object_ = 0x28;
        private static uint last_active_object = 0x20;

        internal static ulong GetGameObjectManager()
        {
            var address = Memory.Read<ulong>(Memory.moduleBaseAddress + ModuleEFT.offsetsEFT.GOM_Offset, false, false);
            gameObjectManagerAddress = address;
                        return address;
        }

        internal static BaseObject GetTaggedObjects()
        {
            return Memory.Read<BaseObject>(gameObjectManagerAddress + tagged_object_);
        }

        internal static BaseObject GetActiveObjects()
        {
            return Memory.Read<BaseObject>(gameObjectManagerAddress + active_object_);
        }

        internal static FPSCamera GetFPSCamera()
        {
            if (ModuleEFT.readerEFT.GameWorldValid() == false)
            {
                return null;
            }

            var q = GetTaggedObjectByName("FPS Camera");

            if (q == null)
            {
                return new FPSCamera(new GameObject(0));
            }

            
            var w = new FPSCamera(q);
            return w;
        }

        internal static ulong GetGameWorld()
        {
            var q = GetActiveObjectByName("GameWorld", 1, 1500, 50);

            if (q == null)
            {
                return 0;
            }

                        var a = q.address;
            
            return a;
        }

        internal static MainApplicationObject GetMainApplication()
        {
            var q = GetActiveObjectByName("Application (Main Client)", 1, 1500, 0);

            if (q == null)
            {
                return new MainApplicationObject(new GameObject(0));
            }

            
            var w = new MainApplicationObject(q);
            return w;
        }

        internal static List<GameObject> GetActiveObjectsByName(string objNameToFind, int howMany, int limit = 0, int skip = 0)
        {
            if (Memory.IsValidPointer(gameObjectManagerAddress) == false)
            {
                                return default;
            }

            var pCurrentGameObject = GetActiveObjects();

            if (Memory.IsValidPointer(pCurrentGameObject.baseObjectAddress) == false)
            {
                
                return default;
            }

            return GetObjectsByName(objNameToFind, pCurrentGameObject, howMany, limit, skip);
        }

        internal static GameObject GetActiveObjectByName(string objNameToFind, int selectCount = 2, int limit = 0, int skip = 0)
        {
            if (Memory.IsValidPointer(gameObjectManagerAddress) == false)
            {
                                return default;
            }

            var pCurrentGameObject = GetActiveObjects();

            if (Memory.IsValidPointer(pCurrentGameObject.baseObjectAddress) == false)
            {
                
                return default;
            }

            return GetObjectByName(objNameToFind, pCurrentGameObject, selectCount, limit, skip);
        }

        internal static GameObject GetTaggedObjectByName(string objNameToFind, int selectCount = 1, int limit = 1800, int skip = 0)
        {
            if (Memory.IsValidPointer(gameObjectManagerAddress) == false)
            {
                                return default;
            }

            var pCurrentGameObject = GetTaggedObjects();

            if (Memory.IsValidPointer(pCurrentGameObject.baseObjectAddress) == false)
            {
                                return default;
            }

            return GetObjectByName(objNameToFind, pCurrentGameObject, selectCount, limit, skip);
        }

        internal static GameObject GetLastActiveObject()
        {
            var objectBase = Memory.Read<ulong>(gameObjectManagerAddress + last_active_object);
            return new GameObject(Memory.Read<ulong>(objectBase + last_active_object));
        }

        private static GameObject GetObjectByName(string objNameToFind, BaseObject baseObject, int selectCount, int limit, int skip = 0)
        {
            var currentFoundCount = 0;

            string objectName = "n/a";

            var lastActiveObject = GetLastActiveObject();

            for (uint curObject = 0; curObject < limit; curObject++)
            {
                if (Memory.IsValidPointer(baseObject.baseObjectAddress) == false)
                {
                                        return default;
                }

                if (curObject < skip)
                {
                                        baseObject = Memory.Read<BaseObject>(baseObject.baseObjectAddress + 0x8);
                    continue;
                }

                var gameObject = baseObject.GetThis();

                if (Memory.IsValidPointer(gameObject.address) == false)
                {
                    return default;
                }

                if (lastActiveObject.address == gameObject.address)
                {
                                        return default;
                }

                objectName = gameObject.GetName();
                
                if (string.Compare(objectName, objNameToFind, true) == 0)
                {
                    currentFoundCount++;
                    
                    if (selectCount == currentFoundCount)
                    {
                                                gameObject.foundInCurObject = curObject;
                        return gameObject;
                    }
                }

                baseObject = baseObject.GetNext();
            }

                        return default;
        }

        private static List<GameObject> GetObjectsByName(string objNameToFind, BaseObject baseObject, int howMany, int limit, int skip = 0)
        {
            string objectName = "n/a";
            List<GameObject> gameObjectsList = new List<GameObject>();

            var lastActiveObject = GetLastActiveObject();

            for (uint curObject = 0; curObject < limit; curObject++)
            {
                if (Memory.IsValidPointer(baseObject.baseObjectAddress) == false)
                {
                                        return default;
                }

                if (curObject < skip)
                {
                                        baseObject = Memory.Read<BaseObject>(baseObject.baseObjectAddress + 0x8);
                    continue;
                }

                var gameObject = baseObject.GetThis();

                if (Memory.IsValidPointer(gameObject.address) == false)
                {
                    continue;
                }

                if (lastActiveObject.address == gameObject.address)
                {
                                        continue;
                }

                objectName = gameObject.GetName();
                
                if (string.Compare(objectName, objNameToFind, true) == 0)
                {
                                        gameObject.foundInCurObject = curObject;
                    gameObjectsList.Add(gameObject);

                    if (gameObjectsList.Count >= howMany)
                    {
                        return gameObjectsList;
                    }
                }

                baseObject = baseObject.GetNext();
            }

                        return gameObjectsList;
        }

        internal static void GetAllObjects()
        {
        }
    }
}