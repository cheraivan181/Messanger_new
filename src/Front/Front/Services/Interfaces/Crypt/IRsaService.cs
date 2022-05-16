namespace Front.Services.Interfaces.Crypt
{
    public interface IRsaService
    {
        Task<(string privateKey, string publicKey)> GetRsaKeysAsync();
        Task<string> CryptTextAsync(string textToCrypt);
        Task<string> DecryptTextAsync(string cryptedText);
        Task<string> DecryptTextAsync(string privateKey, string cryptedText);
    }
}
