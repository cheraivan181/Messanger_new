namespace Core.CryptService.Interfaces;

public interface IHmacService
{
    string GetSignature(string secret, string dataToSign);
}