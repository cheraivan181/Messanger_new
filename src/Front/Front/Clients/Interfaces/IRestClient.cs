using System.Net;
using Front.Domain.Responses.Base;

namespace Front.Clients.Interfaces;

public interface IRestClient
{
    Task<RestClientResponse<T>> MakeHttpRequestAsync<T>(string uri, HttpMethod httpMethod,
        HttpStatusCode exceptedStatusCode = HttpStatusCode.OK, object data = null) where T : class;
    
    Task<RestClientResponseWithoutResult> MakeHttpRequestWithoutResponseAsync(string uri, HttpMethod httpMethod,
        HttpStatusCode exceptedStatusCode = HttpStatusCode.OK, object data = null);
}