namespace Front.Services.Interfaces.Crypt
{
    public interface IAesCryptService
    {
        Task<string> CryptText(Guid dialogId, string textToCrypt);
        Task<string> DecryptText(Guid dialogId, string cipherText);
    }
}
