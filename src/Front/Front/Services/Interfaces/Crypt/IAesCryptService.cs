namespace Front.Services.Interfaces.Crypt
{
    public interface IAesCryptService
    {
        string DecryptText(string cipherText, byte[] Key, byte[] IV);
        string CryptText(string plainText, byte[] Key, byte[] IV);
    }
}
