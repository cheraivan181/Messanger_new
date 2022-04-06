using Front.Clients.Interfaces;
using Front.ClientsDomain.Requests.Session;
using Front.ClientsDomain.Responses.Session;
using Front.Domain.Responses.Base;

namespace Front.Clients.Implementations
{
    public class SessionClient : ISessionClient
    {
        private readonly IRestClient _restClient;

        public SessionClient(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<RestClientResponse<CreateSessionResponse>> CreateSessionAsync(string publicKey)
        {
            var createSessionRequest = new CreateSessionRequest()
            {
                PublicKey = publicKey
            };

            var response = await _restClient
                .MakeHttpRequestAsync<CreateSessionResponse>("Session/createSession", HttpMethod.Post, data: createSessionRequest);

            return response;
        }
    }
}
