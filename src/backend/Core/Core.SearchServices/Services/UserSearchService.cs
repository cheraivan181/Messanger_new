using Core.Repositories.Interfaces;
using Core.SearchServices.Domain;
using Core.SearchServices.Interfaces;
using Serilog;

namespace Core.SearchServices.Services;

public class UserSearchService : IUserSearchService
{
    private readonly IUserRepository _userRepository;
    private readonly IDialogRepository _dialogRepository;

    public UserSearchService(IUserRepository userRepository,
        IDialogRepository dialogRepository)
    {
        _userRepository = userRepository;
        _dialogRepository = dialogRepository;
    }

    public async Task<SearchUserResult> SearchUsersAsync(Guid requestUserId, string requestUserName, string predicate)
    {
        Log.Debug($"Method {nameof(SearchUsersAsync)} was started, requestUserId #({requestUserId}), " +
                  $"predicate #({predicate})");
        
        var result = new SearchUserResult();
        var searchModels = new List<SearchUserModel>();
        
        var users = await _userRepository.SearchUsersByUserNameAsync(requestUserId, predicate);

        foreach (var user in users)
        {
            searchModels.Add(new SearchUserModel(user.Id, user.UserName, false));
        }
        
        result.SetSucessResult(searchModels); 
        return result;
    }
}