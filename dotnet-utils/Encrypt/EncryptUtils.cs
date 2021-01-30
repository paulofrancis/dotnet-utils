using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace dotnet_utils.Encrypt
{
    public class EncryptUtils
    {
        public static string Encrypt(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string password = "abcdzywz";
                                
                byte[] utfData = Encoding.UTF8.GetBytes(input);
                byte[] saltBytes = Encoding.UTF8.GetBytes(password);
                
                string encryptedString;
                using (var aes = new AesManaged())
                {
                    var rfc = new Rfc2898DeriveBytes(password, saltBytes);

                    aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                    aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                    aes.Key = rfc.GetBytes(aes.KeySize / 8);
                    aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                    using var encryptTransform = aes.CreateEncryptor();
                    using var encryptedStream = new MemoryStream();
                    using var encryptor = new CryptoStream(encryptedStream, encryptTransform, CryptoStreamMode.Write);
                    encryptor.Write(utfData, 0, utfData.Length);
                    encryptor.Flush();
                    encryptor.Close();

                    byte[] encryptBytes = encryptedStream.ToArray();
                    encryptedString = Convert.ToBase64String(encryptBytes);
                }

                encryptedString = encryptedString.Replace("+", "-").Replace("/", "_");

                return encryptedString;
            }
            else
            {
                return null;
            }
        }

        public static string Decrypt(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string password = "abcdzywz";
                input = input.Replace("-", "+").Replace("_", "/");

                byte[] encryptedBytes = Convert.FromBase64String(input);
                byte[] saltBytes = Encoding.UTF8.GetBytes(password);
                
                string decryptedString;
                using (var aes = new AesManaged())
                {
                    var rfc = new Rfc2898DeriveBytes(password, saltBytes);
                    aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                    aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                    aes.Key = rfc.GetBytes(aes.KeySize / 8);
                    aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                    using var decryptTransform = aes.CreateDecryptor();
                    using var decryptedStream = new MemoryStream();
                    var decryptor =
                        new CryptoStream(decryptedStream, decryptTransform, CryptoStreamMode.Write);
                    decryptor.Write(encryptedBytes, 0, encryptedBytes.Length);
                    decryptor.Flush();
                    decryptor.Close();

                    byte[] decryptBytes = decryptedStream.ToArray();
                    decryptedString = Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
                }

                return decryptedString;
            }
            else
            {
                return null;
            }
        }

        public static string SH1Encrypt(string input)
        {
            string password = ".abcdzywz";

            SHA1 hash = new SHA1CryptoServiceProvider();
            SHA1 hash2 = new SHA1CryptoServiceProvider();

            hash.ComputeHash(Encoding.Unicode.GetBytes(input));

            string pass2 = BitConverter.ToString(hash.Hash).Replace("-", "");
            hash2.ComputeHash(Encoding.Unicode.GetBytes(pass2 + password));

            return BitConverter.ToString(hash2.Hash).Replace("-", "");
        }

        public static string SH1EncryptASCII(string password)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            string hash1 = BitConverter.ToString(new SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(password))).Replace("-", "").ToLower();
            string hash2 = BitConverter.ToString(new SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(hash1 + ".abcdzywz"))).Replace("-", "").ToLower();
            return hash2;
        }

        public static string GetMD5(string text)
        {
            var md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(Encoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;
            StringBuilder str = new StringBuilder();

            for (int i = 1; i < result.Length; i++)
            {
                str.Append(result[i].ToString("x2"));
            }

            return str.ToString();
        }
    }
}
