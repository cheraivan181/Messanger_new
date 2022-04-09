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
        
        var users = await _userRepository.SearchUsersByUserNameAsync(predicate);
        var dialogAndDialogRequests = await _dialogRepository
            .GetUserDialogsAndDialogRequestsAsync(requestUserId, predicate);
        
        foreach (var dialog in dialogAndDialogRequests.dialogs)
        {
            string userName = string.Empty;
            if (!string.IsNullOrEmpty(dialog.User1.UserName) && dialog.User1.UserName != requestUserName)
                userName = dialog.User1.UserName;
            else if (!string.IsNullOrEmpty(dialog.User2?.UserName) && dialog.User2.UserName != requestUserName)
                userName = dialog.User2.UserName;
            else
                continue;

            searchModels.Add(new SearchUserModel(userName, true));
        }
        
        foreach (var dialogRequest in dialogAndDialogRequests.dialogRequest)
        {
            string userName = dialogRequest.RequestUser.UserName;
            if (string.IsNullOrEmpty(userName))
            {
                result.SetServerError();
                return result;
            }

            searchModels.Add(new SearchUserModel(userName, false));
        }

        var alredyAddedUsers = searchModels.Select(x => x.UserName);
        foreach (var user in users.Where(x => !alredyAddedUsers.Contains(x.UserName) && x.UserName != requestUserName))
        {
            searchModels.Add(new SearchUserModel(user.UserName, false));
        }
        
        result.SetSucessResult(searchModels); 
        return result;
    }
}