using Core.ApiResponses.Search;
using Core.Mapping.Interfaces;
using Core.SearchServices.Domain;

namespace Core.Mapping.Impl;

public class SearchMapper : ISearchMapper
{
    public SearchUserResponse MapSearchUserResponse(List<SearchUserModel> result)
    {
        var response = new SearchUserResponse();
        response.SearchUserResults = result;
        response.IsFoundUsers = result.Count > 0;

        return response;
    }
}