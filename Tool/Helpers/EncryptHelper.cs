using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Tool.Helpers
{
    public class EncryptHelper
    {
        private Aes AesHandler { get; }

        public EncryptHelper()
        {
        }
        public EncryptHelper(string key,string iv)
        {
            AesHandler = Aes.Create();
            AesHandler.Key = Convert.FromBase64String(key);
            AesHandler.IV = Convert.FromBase64String(iv);
        }

        public string Encrypt(string source)
        {
            using var mem = new MemoryStream();
            using var stream = new CryptoStream(mem, AesHandler.CreateEncryptor(AesHandler.Key, AesHandler.IV),
                CryptoStreamMode.Write);
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(source);
            }
            return Convert.ToBase64String(mem.ToArray());
        }

        public string Decrypt(string source)
        {
            var data = Convert.FromBase64String(source);
            using var mem = new MemoryStream(data);
            using var crypto = new CryptoStream(mem, AesHandler.CreateDecryptor(AesHandler.Key, AesHandler.IV),
                CryptoStreamMode.Read);
            using var reader = new StreamReader(crypto);
            return reader.ReadToEnd();
        }

        public  string ShaEncrypt(string source)
        {
            using SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(source);
            byte[] sha256Bytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(sha256Bytes);
        }
    }
}