using Core.SearchServices.Domain;

namespace Core.SearchServices.Interfaces;

public interface IUserSearchService
{
    Task<SearchUserResult> SearchUsersAsync(long requestUserId, string requestUserName, string predicate);
}