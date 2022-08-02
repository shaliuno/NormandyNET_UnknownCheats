using NormandyNET.Core;
using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace NormandyNET
{
    internal static class CommonHelpers
    {
        internal static DateTime dateTimeHolder = DateTime.UtcNow;

        public static bool Positive(this float value)
        {
            return value >= float.Epsilon;
        }

        internal static void NOP(int ticks = 10000)
        {
            var freq = System.Diagnostics.Stopwatch.Frequency;

            var frame = System.Diagnostics.Stopwatch.GetTimestamp();
            var frameNew = frame + ticks;

            while (true)
            {
                frame = System.Diagnostics.Stopwatch.GetTimestamp();
                if (frame > frameNew) { break; }
            }
        }

        public static T GetValueFromName<T>(this string name) where T : Enum
        {
            var type = typeof(T);

            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute)
                {
                    if (attribute.Name == name)
                    {
                        return (T)field.GetValue(null);
                    }
                }

                if (field.Name == name)
                {
                    return (T)field.GetValue(null);
                }
            }

            throw new ArgumentOutOfRangeException(nameof(name));
        }

        internal static string GetFileHashMD5(string path)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        internal static string GetDistanceUnit(float meters)
        {
            if (meters < 1000)
            {
                return $"{meters}m ";
            }
            else
            {
                var km = (float)Math.Round(meters / 1000, 1);
                return $"{km}km ";
            }
        }

        internal static string GetElevationData(double elevation)
        {
            var elevationDiff = (int)Math.Round(elevation / 4, 0);
            var strSigh = ' ';
            var renderGlyph = false;

            if (elevationDiff > 0)
            {
                if (elevationDiff > 3)
                {
                    elevationDiff = 3;
                }

                strSigh = (char)9650;

                renderGlyph = true;
            }

            if (elevationDiff < 0)
            {
                if (elevationDiff < -3)
                {
                    elevationDiff = -3;
                }
                strSigh = (char)9660;

                renderGlyph = true;
            }

            if (renderGlyph)
            {
                if (strSigh == (char)9650)
                {
                    return $"©{CommonHelpers.ColorHexConverter(Color.LimeGreen)}{strSigh}";
                }
                if (strSigh == (char)9660)
                {
                    return $"©{CommonHelpers.ColorHexConverter(Color.Red)}{strSigh}";
                }
            }
            return "";
        }

        public static string GetStringFromMemory_Unity(ulong address, bool unicode)
        {
            var count = Memory.Read<int>(address + 0x10);
            return GetStringFromMemory(address + 0x14, true, count);
        }

        public static string GetStringFromMemory(ulong address, bool unicode, int size = 32)
        {
            if (size <= 0 || size > 500)
            {
                return "n/a";
            }

            if (unicode)
            {
                size *= 2;
            }

            var stringBytes = Memory.ReadBytes((ulong)address, size);
            var clearBytes = new List<byte>();

            var stop = false;
            for (int i = 0; i < stringBytes.Count(); i++)
            {
                if (stringBytes[i] == 0x0 && unicode && stop)
                {
                    break;
                }

                stop = stringBytes[i] == 0x0 && unicode && !stop ? true : false;

                if (stringBytes[i] == 0x0 && !unicode)
                {
                    break;
                }

                clearBytes.Add(stringBytes[i]);
            }

            if (clearBytes.Count <= 1)
            {
                return "n/a";
            }

            if (clearBytes.Count % 2 != 0 && unicode)
            {
                clearBytes.Add(0x00);
            }

            return unicode ? Encoding.Unicode.GetString(clearBytes.ToArray()) : Encoding.ASCII.GetString(clearBytes.ToArray());
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) { return value; }

            return value.Substring(0, Math.Min(value.Length, maxLength));
        }

        public static bool PlayerListToRenderComplete = false;

        internal static String ColorHexConverter(System.Drawing.Color c)
        {
            return "#" + c.A.ToString("X2") + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static string GetMD5HashFromFile(string file)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file))
                {
                    byte[] checksum = md5.ComputeHash(stream);
                    return BitConverter.ToString(checksum).Replace("-", String.Empty).ToUpper();
                }
            }
        }

        public static int BytePatternSearch(byte[] sourceBytes, byte[] patternBytes)
        {
            int count = sourceBytes.Length - patternBytes.Length + 1;
            int j;
            for (int i = 0; i < count; i++)
            {
                if (sourceBytes[i] != patternBytes[0]) continue;
                for (j = patternBytes.Length - 1; j >= 1 && sourceBytes[i + j] == patternBytes[j]; j--) ;
                if (j == 0) return i;
            }
            return -1;
        }

        public static float myIngamePositionX, myIngamePositionZ, myIngamePositionY;

        public static float D3DXVec3Dot(Vector3 a, Vector3 b)
        {
            return (a.X * b.X) +
                    (a.Y * b.Y) +
                    (a.Z * b.Z);
        }

        public static Vector3 openGLVector(UnityEngine.Vector3 unityVector)
        {
            return new OpenTK.Vector3(unityVector.x, unityVector.z, unityVector.y);
        }

        public static float GetDistance(Vector3 v1, Vector3 v2)
        {
            Vector3 difference = new Vector3(
              v1.X - v2.X,
              v1.Y - v2.Y,
              v1.Z - v2.Z);

            double distance = Math.Sqrt(
                              Math.Pow(difference.X, 2f) +
                              Math.Pow(difference.Y, 2f) +
                              Math.Pow(difference.Z, 2f));

            return (float)distance;
        }

        public static double GetAnglePoints(double start1x, double start1y, double first2x, double first2y, double second3x, double second3y)
        {
            double numerator = (first2y * (start1x - second3x)) + (start1y * (second3x - first2x)) + (second3y * (first2x - start1x));
            double denominator = ((first2x - start1x) * (start1x - second3x)) + ((first2y - start1y) * (start1y - second3y));
            double ratio = numerator / denominator;

            double angleRad = Math.Atan(ratio);
            double angleDeg = (angleRad * 180) / Math.PI;

            if (angleDeg < 0)
            {
                angleDeg = 360 + angleDeg;
            }

            return angleDeg;
        }

        public static void checkCollision(int a, int b, int c,
                            int x, int y, int radius)
        {
            double dist = (Math.Abs(a * x + b * y + c)) /
                            Math.Sqrt(a * a + b * b);
        }

        public static String FixedStr(this string s, int length, char padChar = ' ')
        => String.IsNullOrEmpty(s) ? new string(padChar, length) : s.Length > length ? s.Remove(length) : s.Length < length ? s.PadRight(length) : s;

        public static string CleanInput(this string strIn)
        {
            try
            {
                return Regex.Replace(strIn, @"[^\w\.@-]", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        public static double GetAngleVectors(Vector3 start, Vector3 first, Vector3 second)
        {
            double numerator = (first.Y * (start.X - second.X)) + (start.Y * (second.X - first.X)) + (second.Y * (first.X - start.X));
            double denominator = ((first.X - start.X) * (start.X - second.X)) + ((first.Y - start.Y) * (start.Y - second.Y));
            double ratio = numerator / denominator;

            double angleRad = Math.Atan(ratio);
            double angleDeg = (angleRad * 180) / Math.PI;

            if (angleDeg < 0)
            {
                angleDeg = 180 + angleDeg;
            }

            return angleDeg;
        }

        internal static bool IsValidPort(string port, out int portOut)
        {
            var result = UInt16.TryParse(port, out ushort portOutPre);
            portOut = (int)portOutPre;
            return result;
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        public static byte[] StringToByteArray(String hexPre)
        {
            string hex = hexPre.Replace(" ", string.Empty);
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string ByteArrayToString(byte[] ba, bool spaces = false, int size = -1)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);

            if (size == -1)
            {
                size = ba.Length;
            }

            for (int i = 0; i < size; i++)
            {
                if (spaces)
                {
                    hex.AppendFormat("{0:x2} ", ba[i]);
                }
                else
                {
                    hex.AppendFormat("{0:x2}", ba[i]);
                }
            }

            return hex.ToString();
        }

        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string Decrypt(string rawData, string keyString)
        {
            byte[] key = CommonHelpers.StringToByteArray(keyString);

            var base64 = CommonHelpers.StringToByteArray(rawData);

            var cipherData = Convert.FromBase64String(Encoding.UTF8.GetString(base64));

            byte[] iv = cipherData.Take(16).ToArray();
            byte[] data = cipherData.Skip(16).ToArray();

            try
            {
                using (var memoryStream =
                new MemoryStream(data))

                using (var rijndaelManaged =
                       new RijndaelManaged { Key = key, IV = iv, Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 })

                using (var cryptoStream =
                       new CryptoStream(memoryStream,
                           rijndaelManaged.CreateDecryptor(key, iv),
                           CryptoStreamMode.Read))
                {
                    return new StreamReader(cryptoStream).ReadToEnd();
                }
            }
            catch (CryptographicException e)
            {
                return null;
            }
        }

        public static byte[] Reverse(this byte[] b)
        {
            Array.Reverse(b);
            return b;
        }

        public static UInt16 ReadUInt16BE(this BinaryReader binRdr)
        {
            return BitConverter.ToUInt16(binRdr.ReadBytesRequired(sizeof(UInt16)).Reverse(), 0);
        }

        public static Int16 ReadInt16BE(this BinaryReader binRdr)
        {
            return BitConverter.ToInt16(binRdr.ReadBytesRequired(sizeof(Int16)).Reverse(), 0);
        }

        public static UInt32 ReadUInt32BE(this BinaryReader binRdr)
        {
            return BitConverter.ToUInt32(binRdr.ReadBytesRequired(sizeof(UInt32)).Reverse(), 0);
        }

        public static Int32 ReadInt32BE(this BinaryReader binRdr)
        {
            return BitConverter.ToInt32(binRdr.ReadBytesRequired(sizeof(Int32)).Reverse(), 0);
        }

        public static byte[] ReadBytesRequired(this BinaryReader binRdr, int byteCount)
        {
            var result = binRdr.ReadBytes(byteCount);

            if (result.Length != byteCount)
                throw new EndOfStreamException(string.Format("{0} bytes required from stream, but only {1} returned.", byteCount, result.Length));

            return result;
        }

        private static uint ConvertMegabytesToBytes(uint megabytes)
        {
            return (megabytes * 1024) * 1024;
        }

        public static bool IsValidIP(string ipAddress)
        {
            IPAddress unused;
            return IPAddress.TryParse(ipAddress, out unused) && (unused.AddressFamily != AddressFamily.InterNetwork || ipAddress.Split('.').Length == 4);
        }
    }
}