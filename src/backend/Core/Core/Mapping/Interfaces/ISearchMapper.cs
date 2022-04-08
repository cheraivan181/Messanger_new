using Core.ApiResponses.Search;
using Core.SearchServices.Domain;

namespace Core.Mapping.Interfaces;

public interface ISearchMapper
{
    SearchUserResponse MapSearchUserResponse(List<SearchUserModel> result);
}