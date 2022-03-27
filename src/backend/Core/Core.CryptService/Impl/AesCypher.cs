using System.Security.Cryptography;
using System.Text;
using Core.CryptService.Interfaces;
using Core.Utils;

namespace Core.CryptService.Impl
{
    public class AesCypher : IAesCypher
    {
        public string Crypt(string key, string iv, string value)
        {
            var keyBuffer = Convert.FromBase64String(key);
            var ivBuffer = Convert.FromBase64String(iv);

            using Aes aes = Aes.Create();
            aes.Key = keyBuffer;
            aes.IV = ivBuffer;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encrypted;
            
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(value);
                    }
                    encrypted = msEncrypt.ToArray();
                    return Convert.ToBase64String(encrypted);
                }
            }
        }
        
        public string Decrypt(string cryptedText, string key, string iv)
        {
            byte[] keyBuffer = Convert.FromBase64String(key);
            byte[] ivBuffer = Convert.FromBase64String(iv);
            byte[] cryptedTextBuffer = Convert.FromBase64String(cryptedText);

            return Decrypt(cryptedTextBuffer, keyBuffer, ivBuffer);
        }

        public string Decrypt(byte[] cryptedTextBuffer, byte[] keyBuffer, byte[] ivBuffer)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBuffer;
                aesAlg.IV = ivBuffer;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cryptedTextBuffer))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            var decryptedText = srDecrypt.ReadToEnd();
                            return decryptedText;
                        }
                    }
                }
            }
        }

        public (string key, string iv) GetAesKeyAndIv()
        {
            using Aes aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();
            return (key: Convert.ToBase64String(aes.Key), iv: Convert.ToBase64String(aes.IV));
        }
    }
}
