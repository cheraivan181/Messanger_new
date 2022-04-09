using Front.ClientsDomain.Responses.Search;
using Front.Domain.Responses.Base;

namespace Front.Clients.Interfaces
{
    public interface ISearchClient
    {
        Task<RestClientResponse<SearchUserResponse>> SearchUserByUserNameAsync(string userName);
    }
}
