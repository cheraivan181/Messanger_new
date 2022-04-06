using Front.Clients.Interfaces;
using Front.ClientsDomain.Requests.Crypt;
using Front.ClientsDomain.Responses.Crypt;
using Front.Domain.Responses.Base;

namespace Front.Clients.Implementations
{
    public class CryptClient : ICryptClient
    {
        private readonly IRestClient _restClient;

        public CryptClient(IRestClient restClient) => 
            _restClient = restClient;


        public async Task<RestClientResponse<GetRsaKeysResponse>> GetRsaKeysAsync()
        {
            var response = await _restClient.MakeHttpRequestAsync<GetRsaKeysResponse>("Crypt/getRsaKeys", HttpMethod.Get);
            return response;
        }

        public async Task<RestClientResponse<RsaCryptResponse>> RsaCryptAsync(string publicKey, string text)
        {
            var request = new RsaCryptRequest()
            {
                PublicKey = publicKey,
                Text = text
            };

            var response = await _restClient
                .MakeHttpRequestAsync<RsaCryptResponse>("Crypt/rsaCrypt", HttpMethod.Post, data: request);
            return response;
        }

        public async Task<RestClientResponse<RsaDecryptResponse>> RsaDecryptAsync(string privateKey, string cryptedText)
        {
            var request = new RsaDecryptRequest()
            {
                CryptedText = cryptedText,
                PrivateKey = privateKey
            };

            var response = await _restClient
                .MakeHttpRequestAsync<RsaDecryptResponse>("Crypt/rsaDecrypt", HttpMethod.Post, data: request);

            return response;
        }

        public async Task<RestClientResponse<AesKeyAndIvResponse>> GetAesKetAndIvAsync()
        {
            var result = await _restClient.MakeHttpRequestAsync<AesKeyAndIvResponse>("Crypt/getaeskey", HttpMethod.Get);
            return result;
        }

        public async Task<RestClientResponse<AesCryptResult>> AesCryptAsync(string key, string iv, string text)
        {
            var request = new AesCryptRequest()
            {
                Key = key,
                IV = iv,
                Text = text
            };

            var result = await _restClient
                .MakeHttpRequestAsync<AesCryptResult>("Crypt/aesCrypt", HttpMethod.Post, data: request);
            return result;
        }

        public async Task<RestClientResponse<AesDecryptResult>> AesDecryptAsync(string key, string iv, string cryptedText)
        {
            var request = new AesDecryptRequest()
            {
                CryptedText = key,
                IV = iv,
                CypherKey = cryptedText
            };

            var result = await _restClient
                .MakeHttpRequestAsync<AesDecryptResult>("Crypt/aesDecrypt", HttpMethod.Post, data: request);
            return result;
        }
    }
}
