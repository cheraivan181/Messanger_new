using Core.CryptService.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Core.CryptService.Impl
{
    public class HashService : IHashService
    {
        public string GetHash(string data)
        {
            using var sha512 = SHA512.Create();
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            byte[] hash = sha512.ComputeHash(buffer);

            var result = GetStringFromHash(hash);
            return result;
        }

        private string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }
}
