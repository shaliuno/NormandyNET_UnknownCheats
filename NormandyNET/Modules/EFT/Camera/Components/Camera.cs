using NormandyNET.Core;
using System.Numerics;

namespace NormandyNET.Modules.EFT.Objects.Components
{
    internal class Camera : GameObjectComponent
    {
        private Matrix4x4 viewMatrix;
        private float viewFov;

        public Camera(object[] args) : base(args)
        {
                                }

        internal Matrix4x4 GetViewMatrix(bool read = true)
        {
            if (read == false)
            {
                return viewMatrix;
            }

            if (Memory.IsValidPointer(component) == false)
            {
                return default;
            }

            var q = Memory.Read<Matrix4x4>(component + ModuleEFT.offsetsEFT.CameraMatrix);
            viewMatrix = q;
            return q;
        }

        internal float GetAspect()
        {
            if (Memory.IsValidPointer(component) == false)
            {
                return default;
            }

            var q = Memory.Read<float>(component + ModuleEFT.offsetsEFT.CameraAspect);
            return q;
        }

        internal float GetViewFov(bool read = true)
        {
            if (read == false)
            {
                return viewFov;
            }

            if (Memory.IsValidPointer(component) == false)
            {
                return 50f;
            }

            var q = Memory.Read<float>(component + ModuleEFT.offsetsEFT.CameraFOV);
            viewFov = q;

            return q;
        }
    }
}