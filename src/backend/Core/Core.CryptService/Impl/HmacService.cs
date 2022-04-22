using System.Security.Cryptography;
using System.Text;
using Core.CryptService.Interfaces;

namespace Core.CryptService.Impl;

public class HmacService : IHmacService
{
    public string GetSignature(string secret, string dataToSign)
    {
        using var hmac = new HMACSHA256(Convert.FromBase64String(secret));
        byte[] buffer = Encoding.UTF8.GetBytes(dataToSign);
        var hashedBytes = hmac.ComputeHash(buffer);

        return Convert.ToBase64String(hashedBytes);
    }
}