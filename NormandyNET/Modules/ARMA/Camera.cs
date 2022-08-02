using NormandyNET.Core;
using System;
using System.Numerics;

namespace NormandyNET.Modules.ARMA
{
    internal class Camera
    {
        internal static ulong camera;

        internal static Vector3 ViewRight;
        internal static Vector3 ViewUp;
        internal static Vector3 ViewForward;
        internal static Vector3 ViewTranslation;
        internal static Vector3 ViewportSize;
        internal static Vector3 ProjectionD1;
        internal static Vector3 ProjectionD2;

        internal Camera()
        {
        }

        internal static void GetCameraRelated()
        {
            
            if (camera == 0)
            {
                return;
            }

            var databuffer = Memory.ReadBytes(camera, sizeof(float) * 58);

            ViewRight.X = BitConverter.ToSingle(databuffer, 0x8);
            ViewRight.Y = BitConverter.ToSingle(databuffer, 0x8 + 0x4);
            ViewRight.Z = BitConverter.ToSingle(databuffer, 0x8 + 0x4 + 0x4);

            ViewUp.X = BitConverter.ToSingle(databuffer, 0x14);
            ViewUp.Y = BitConverter.ToSingle(databuffer, 0x14 + 0x4);
            ViewUp.Z = BitConverter.ToSingle(databuffer, 0x14 + 0x4 + 0x4);

            ViewForward.X = BitConverter.ToSingle(databuffer, 0x20);
            ViewForward.Y = BitConverter.ToSingle(databuffer, 0x20 + 0x4);
            ViewForward.Z = BitConverter.ToSingle(databuffer, 0x20 + 0x4 + 0x4);

            ViewTranslation.X = BitConverter.ToSingle(databuffer, 0x2C);
            ViewTranslation.Y = BitConverter.ToSingle(databuffer, 0x2C + 0x4);
            ViewTranslation.Z = BitConverter.ToSingle(databuffer, 0x2C + 0x4 + 0x4);

            ViewportSize.X = BitConverter.ToSingle(databuffer, 0x58);
            ViewportSize.Y = BitConverter.ToSingle(databuffer, 0x58 + 0x4);
            ViewportSize.Z = BitConverter.ToSingle(databuffer, 0x58 + 0x4 + 0x4);

            ProjectionD1.X = BitConverter.ToSingle(databuffer, 0xD0);
            ProjectionD1.Y = BitConverter.ToSingle(databuffer, 0xD0 + 0x4);
            ProjectionD1.Z = BitConverter.ToSingle(databuffer, 0xD0 + 0x4 + 0x4);

            ProjectionD2.X = BitConverter.ToSingle(databuffer, 0xDC);
            ProjectionD2.Y = BitConverter.ToSingle(databuffer, 0xDC + 0x4);
            ProjectionD2.Z = BitConverter.ToSingle(databuffer, 0xDC + 0x4 + 0x4);
                    }

        internal static Vector3 WorldToScreen(Vector3 position)
        {
            if (!Memory.IsValidPointer(camera))
            {
                return Vector3.Zero;
            }

            Vector3 temp = position - ViewTranslation;

            float x = Vector3.Dot(temp, ViewRight);
            float y = Vector3.Dot(temp, ViewUp);
            float z = Vector3.Dot(temp, ViewForward);

            return new Vector3(
               ((ModuleARMA.radarForm.overlay.Width / 2) * (1 + (x / ProjectionD1.X / z))) - (ModuleARMA.radarForm.overlay.Width / 2),
               ((ModuleARMA.radarForm.overlay.Height / 2) * (1 - (y / ProjectionD2.Y / z))) - (ModuleARMA.radarForm.overlay.Height / 2),
               z);
        }

        internal static Vector3 GetViewRight()
        {
            if (!Memory.IsValidPointer(camera))
            {
                return Vector3.Zero;
            }
            return Memory.Read<Vector3>(camera + ModuleARMA.offsetsARMA.camera_viewright);
        }

        internal static Vector3 GetViewUp()
        {
            if (!Memory.IsValidPointer(camera))
            {
                return Vector3.Zero;
            }
            return Memory.Read<Vector3>(camera + ModuleARMA.offsetsARMA.camera_viewup);
        }

        internal static Vector3 GetViewForward()
        {
            if (!Memory.IsValidPointer(camera))
            {
                return Vector3.Zero;
            }
            return Memory.Read<Vector3>(camera + ModuleARMA.offsetsARMA.camera_viewforward);
        }

        internal static Vector3 GetViewTranslation()
        {
            if (!Memory.IsValidPointer(camera))
            {
                return Vector3.Zero;
            }
            return Memory.Read<Vector3>(camera + ModuleARMA.offsetsARMA.camera_viewtranslation);
        }

        internal static Vector3 GetViewportSize()
        {
            if (!Memory.IsValidPointer(camera))
            {
                return Vector3.Zero;
            }
            return Memory.Read<Vector3>(camera + ModuleARMA.offsetsARMA.camera_viewportsize);
        }

        internal static Vector3 GetProjectionD1()
        {
            if (!Memory.IsValidPointer(camera))
            {
                return Vector3.Zero;
            }
            return Memory.Read<Vector3>(camera + ModuleARMA.offsetsARMA.camera_projection_d1);
        }

        internal static Vector3 GetProjectionD2()
        {
            if (!Memory.IsValidPointer(camera))
            {
                return Vector3.Zero;
            }
            return Memory.Read<Vector3>(camera + ModuleARMA.offsetsARMA.camera_projection_d2);
        }
    }
}