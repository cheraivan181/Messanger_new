namespace Front.Services.Interfaces.Crypt
{
    public interface IRsaService
    {
        Task<(string privateKey, string publicKey)> GetRsaKeysAsync();
    }
}
