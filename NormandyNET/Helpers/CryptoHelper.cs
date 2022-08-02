using NormandyNET;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Helpers
{
    public static class CryptoHelper
    {
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

        public static string DecryptNoBase64(byte[] rawData, string keyString)
        {
            byte[] key = CommonHelpers.StringToByteArray(keyString);

            byte[] iv = rawData.Take(16).ToArray();
            byte[] data = rawData.Skip(16).ToArray();

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

        [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);

        public static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    rng.GetBytes(data);
                }
            }

            return data;
        }

        public static void FileDecrypt(string inputFile, string outputFile, string password)
        {
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] salt = new byte[32];

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
            fsCrypt.Read(salt, 0, salt.Length);

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = CipherMode.CFB;

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int read;
            byte[] buffer = new byte[1048576];

            try
            {
                while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fsOut.Write(buffer, 0, read);
                }
            }
            catch (CryptographicException ex_CryptographicException)
            {
            }
            catch (Exception ex)
            {
            }

            try
            {
                cs.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                fsOut.Close();
                fsCrypt.Close();
            }
        }

        public static byte[] FileDecryptInMemory(byte[] byteData, string password)
        {
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] salt = new byte[32];

            MemoryStream fsCrypt = new MemoryStream(byteData);
            fsCrypt.Read(salt, 0, salt.Length);

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = CipherMode.CFB;

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);
            var byteDataOut = new byte[byteData.Length];

            MemoryStream fsOut = new MemoryStream(byteDataOut);

            int read;
            byte[] buffer = new byte[1048576];

            try
            {
                while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fsOut.Write(buffer, 0, read);
                }
            }
            catch (CryptographicException ex_CryptographicException)
            {
            }
            catch (Exception ex)
            {
            }

            try
            {
                cs.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                fsOut.Close();
                fsCrypt.Close();
            }

            return fsOut.ToArray();
        }
    }
}