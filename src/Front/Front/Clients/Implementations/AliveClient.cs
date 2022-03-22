using Front.Clients.Interfaces;
using Front.Domain.Responses;
using Front.Domain.Responses.Base;

namespace Front.Clients.Implementations;

public class AliveClient : IAliveClient
{
    private readonly IRestClient _restClient;

    public AliveClient(IRestClient restClient) =>
        _restClient = restClient;

    public async Task<RestClientResponse<IsAliveResponse>> IsAliveAsync()
    {
        var response = await _restClient.MakeHttpRequestAsync<IsAliveResponse>("Alive", HttpMethod.Get);
        return response;
    }
}