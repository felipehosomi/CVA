using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace B1.WFN.API.Infrastructure
{
    public static class Crypto
    {
        private const string AesKey256 = @"AES-CRYPT0-P0RT4LC0N3CT0RW3B-4P1";

        public static string Encrypt(this string text)
        {
            string key = AesKey256;
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have valid value.", nameof(key));
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("The text must have valid value.", nameof(text));

            var buffer = Encoding.UTF8.GetBytes(text);
            var hash = new SHA512CryptoServiceProvider();
            var aesKey = new byte[24];
            Buffer.BlockCopy(hash.ComputeHash(Encoding.UTF8.GetBytes(key)), 0, aesKey, 0, 24);

            using (var aes = Aes.Create())
            {
                if (aes == null)
                    throw new ArgumentException("Parameter must not be null.", nameof(aes));

                aes.Key = aesKey;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var resultStream = new MemoryStream())
                {
                    using (var aesStream = new CryptoStream(resultStream, encryptor, CryptoStreamMode.Write))
                    using (var plainStream = new MemoryStream(buffer))
                    {
                        plainStream.CopyTo(aesStream);
                    }

                    var result = resultStream.ToArray();
                    var combined = new byte[aes.IV.Length + result.Length];
                    Array.ConstrainedCopy(aes.IV, 0, combined, 0, aes.IV.Length);
                    Array.ConstrainedCopy(result, 0, combined, aes.IV.Length, result.Length);

                    return Convert.ToBase64String(combined);
                }
            }
        }

        public static string Decrypt(this string encryptedText)
        {
            string key = AesKey256;
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have valid value.", nameof(key));
            if (string.IsNullOrEmpty(encryptedText))
                throw new ArgumentException("The encrypted text must have valid value.", nameof(encryptedText));

            var combined = Convert.FromBase64String(encryptedText);
            var buffer = new byte[combined.Length];
            var hash = new SHA512CryptoServiceProvider();
            var aesKey = new byte[24];
            Buffer.BlockCopy(hash.ComputeHash(Encoding.UTF8.GetBytes(key)), 0, aesKey, 0, 24);

            using (var aes = Aes.Create())
            {
                if (aes == null)
                    throw new ArgumentException("Parameter must not be null.", nameof(aes));

                aes.Key = aesKey;

                var iv = new byte[aes.IV.Length];
                var ciphertext = new byte[buffer.Length - iv.Length];

                Array.ConstrainedCopy(combined, 0, iv, 0, iv.Length);
                Array.ConstrainedCopy(combined, iv.Length, ciphertext, 0, ciphertext.Length);

                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var resultStream = new MemoryStream())
                {
                    using (var aesStream = new CryptoStream(resultStream, decryptor, CryptoStreamMode.Write))
                    using (var plainStream = new MemoryStream(ciphertext))
                    {
                        plainStream.CopyTo(aesStream);
                    }

                    return Encoding.UTF8.GetString(resultStream.ToArray());
                }
            }
        }
        public static string Decrypt256(string text)
        {
            // Convert Base64 strings to byte array
            byte[] bText = Convert.FromBase64String(text.Replace("-", "+").Replace("_", "/"));
            byte[] bData = new byte[bText.Length - 16];
            byte[] bIV = new byte[16];

            Array.Copy(bText, 16, bData, 0, bText.Length - 16);
            Array.Copy(bText, 0, bIV, 0, bIV.Length);

            // RijndaelManaged
            using (RijndaelManaged aes = new RijndaelManaged())
            {
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.Mode = CipherMode.CFB;
                aes.Padding = PaddingMode.PKCS7;

                // Hash key to SHA256
                aes.Key = new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(AesKey256));
                aes.IV = bIV;


                using (ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, bIV))
                using (MemoryStream msDecrypt = new MemoryStream(bData))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decrypt, CryptoStreamMode.Read))
                {
                    //bData = AddPkcs7Padding(bData, 16);
                    byte[] fromEncrypt = new byte[bData.Length];

                    //Read the data out of the crypto stream
                    csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                    //Convert the byte array back into a string
                    return Encoding.UTF8.GetString(fromEncrypt).Replace("\0", string.Empty);
                }
            }
        }
    }
}