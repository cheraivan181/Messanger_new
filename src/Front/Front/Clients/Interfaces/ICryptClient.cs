using Front.ClientsDomain.Responses.Crypt;
using Front.Domain.Responses.Base;

namespace Front.Clients.Interfaces
{
    public interface ICryptClient
    {
        Task<RestClientResponse<GetRsaKeysResponse>> GetRsaKeysAsync();
        Task<RestClientResponse<RsaCryptResponse>> RsaCryptAsync(string publicKey, string text);
        Task<RestClientResponse<RsaDecryptResponse>> RsaDecryptAsync(string privateKey, string cryptedText);
        Task<RestClientResponse<AesKeyAndIvResponse>> GetAesKetAndIvAsync();
        Task<RestClientResponse<AesCryptResult>> AesCryptAsync(string key, string iv, string text);
        Task<RestClientResponse<AesDecryptResult>> AesDecryptAsync(string key, string iv, string cryptedText);
    }
}
