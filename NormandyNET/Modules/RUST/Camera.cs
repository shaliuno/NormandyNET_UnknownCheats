using NormandyNET.Core;

namespace NormandyNET.Modules.RUST
{
    internal class Camera
    {
        internal static ulong MainCamera;
        internal static System.Numerics.Matrix4x4 matrix = new System.Numerics.Matrix4x4();

        internal static void GetCameraMatrix()
        {
            
            if (MainCamera == 0)
            {
                GetMainCamera();
            }
            else
            {
                matrix = Memory.Read<System.Numerics.Matrix4x4>(MainCamera + 0x2E4);
            }

                    }

        private static void GetMainCamera()
        {
            if (Pointers.GameWorldValid() == false)
            {
                                return;
            }

            
            var first_obj = Memory.Read<ulong>(Pointers.GameObjectManager + 0x8);
                        
            var object_entry = Memory.Read<ulong>(first_obj + 0x10);
            
            var object_instance = Memory.Read<ulong>(object_entry + 0x30);
            
            MainCamera = Memory.Read<ulong>(object_instance + 0x18);
            
                    }
    }
}