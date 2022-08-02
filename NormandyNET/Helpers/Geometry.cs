using System;
using System.Numerics;

namespace NormandyNET.Helpers
{
    internal static class Geometry
    {
        private static float radiansToDegrees = 57.29578f;

        internal static float CosSinToDegree(double sin, double cos)
        {
            var angle = Math.Atan2(sin, cos);

            angle *= 360 / Math.PI;

            if (angle < 0)
            {
                angle += 360;
            }
            return (float)angle;
        }

        internal static float CalculateDirectionToDegress(Vector3 direction)
        {
            var qwe = new UnityEngine.Vector3();
            var qwe2 = UnityEngine.Vector3.Distance(qwe, qwe);
            
            var X_Rotation = direction.Z;
            var Y_Rotation = direction.X;
            double Rotation_in_Degrees = 0;

            double PI = 3.14159265358979323846;
            if (X_Rotation > 0 && Y_Rotation < 0 || X_Rotation < 0 && Y_Rotation < 0) Rotation_in_Degrees = PI / 2 + Math.Atan(X_Rotation / Y_Rotation);
            if (X_Rotation < 0 && Y_Rotation > 0 || X_Rotation > 0 && Y_Rotation > 0) Rotation_in_Degrees = 3 * PI / 2 + Math.Atan(X_Rotation / Y_Rotation);

            return (float)RadiansToDegress(Rotation_in_Degrees);
        }

        private static double RadiansToDegress(double radians)
        {
            return radians * (180 / Math.PI);
        }

        internal static float NormalizeAngle(float angle)
        {
            while (angle > 360)
                angle -= 360;
            while (angle < 0)
                angle += 360;
            return angle;
        }

        public const float Deg2Rad = (float)Math.PI * 2F / 360F;

        public const float Rad2Deg = 1F / Deg2Rad;

        internal static Vector3 QuaternionToEuler(Quaternion q)
        {
            Vector3 euler = Vector3.Zero;

            float unit = (q.X * q.X) + (q.Y * q.Y) + (q.Z * q.Z) + (q.W * q.W);

            float test = q.X * q.W - q.Y * q.Z;

            if (test > 0.4995f * unit)
            {
                euler.X = (float)Math.PI / 2;
                euler.Y = 2f * (float)Math.Atan2(q.Y, q.X);
                euler.Z = 0;
            }
            else if (test < -0.4995f * unit)
            {
                euler.X = -(float)Math.PI / 2;
                euler.Y = -2f * (float)Math.Atan2(q.Y, q.X);
                euler.Z = 0;
            }
            else
            {
                euler.X = (float)Math.Asin(2f * (q.W * q.X - q.Y * q.Z));
                euler.Y = (float)Math.Atan2(2f * q.W * q.Y + 2f * q.Z * q.X, 1 - 2f * (q.X * q.X + q.Y * q.Y));
                euler.Z = (float)Math.Atan2(2f * q.W * q.Z + 2f * q.X * q.Y, 1 - 2f * (q.Z * q.Z + q.X * q.X));
            }

            euler *= Rad2Deg;

            euler.X %= 360;
            euler.Y %= 360;
            euler.Z %= 360;

            return euler;
        }

        private static float Length(Vector3 v)
        {
            var q = (float)Math.Sqrt(Dot(v, v));
            return q;
        }

        public static float Dot(Vector3 vector1, Vector3 vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        internal static Vector3 CalcAngle(Vector3 Src, Vector3 Dst)
        {
            Vector3 Dir = Src - Dst;

            float Magnitude = Length(Dir);

            var Pitch = (float)Math.Asin(Dir.Y / Magnitude) * radiansToDegrees;
            var Yaw = -(float)Math.Atan2(Dir.X, -Dir.Z) * radiansToDegrees;

            return new Vector3(Yaw, Pitch, 0f);
        }

        internal static double AngleBetween(Vector2 vector1, Vector2 vector2)
        {
            double sin = vector1.X * vector2.Y - vector2.X * vector1.Y;
            double cos = vector1.X * vector2.X + vector1.Y * vector2.Y;

            return Math.Atan2(sin, cos) * (180 / Math.PI);
        }

        internal static double AngleBetweenXZ(Vector3 vector1, Vector3 vector2)
        {
            double sin = vector1.X * vector2.Z - vector2.X * vector1.Z;
            double cos = vector1.X * vector2.X + vector1.Z * vector2.Z;

            return Math.Atan2(sin, cos) * (180 / Math.PI);
        }

        internal static double AngleBetweenXY(Vector3 vector1, Vector3 vector2)
        {
            double sin = vector1.X * vector2.Y - vector2.X * vector1.Y;
            double cos = vector1.X * vector2.X + vector1.Y * vector2.Y;

            return Math.Atan2(sin, cos) * (180 / Math.PI);
        }

        public static float GetDistance(UnityEngine.Vector3 v1, UnityEngine.Vector3 v2)
        {
            UnityEngine.Vector3 difference = new UnityEngine.Vector3(
              v1.x - v2.x,
              v1.y - v2.y,
              v1.z - v2.z);

            double distance = Math.Sqrt(
                              Math.Pow(difference.x, 2f) +
                              Math.Pow(difference.y, 2f) +
                              Math.Pow(difference.z, 2f));

            return (float)distance;
        }
    }
}