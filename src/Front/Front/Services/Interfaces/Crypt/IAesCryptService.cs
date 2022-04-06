namespace Front.Services.Interfaces.Crypt
{
    public interface IAesCryptService
    {
        string DecryptText(long dialogId, string cipherText);
        string CryptText(long dialogId, string textToCrypt);
    }
}
