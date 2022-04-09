using Front.Clients.Interfaces;
using Front.ClientsDomain.Responses.Search;
using Front.Domain.Responses.Base;

namespace Front.Clients.Implementations
{
    public class SearchClient : ISearchClient
    {
        private readonly IRestClient _restClient;

        public SearchClient(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<RestClientResponse<SearchUserResponse>> SearchUserByUserNameAsync(string userName)
        {
            var response = await _restClient
                .MakeHttpRequestAsync<SearchUserResponse>($"Search/searchUser/{userName}", HttpMethod.Get);
            return response;
        }
    }
}
