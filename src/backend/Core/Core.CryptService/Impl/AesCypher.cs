using System.Security.Cryptography;
using System.Text;

namespace Core.CryptService.Impl
{
    public class AesCypher
    {
        public string Crypt(string key, string iv, string value)
        {
            var keyBuffer = Encoding.UTF8.GetBytes(key);
            var ivBuffer = Encoding.UTF8.GetBytes(iv);

            return Crypt(keyBuffer, ivBuffer, value);
        }

        public string Crypt(byte[] key, byte[] iv, string vale)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using var swEncrypt = new StreamWriter(csEncrypt);

            swEncrypt.Write(vale);
            var buffer = msEncrypt.ToArray();
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
