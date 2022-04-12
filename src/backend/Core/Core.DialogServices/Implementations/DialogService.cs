using Core.CryptService.Interfaces;
using Core.DialogServices.Domain;
using Core.DialogServices.Interfaces;
using Core.Repositories.Interfaces;
using Core.SessionServices.Services.Interfaces;
using Serilog;

namespace Core.DialogServices.Implementations;

public class DialogService : IDialogService
{
   private readonly ISessionGetterService _sessionGetterService;
    private readonly IAesCypher _aes;
    private readonly IRsaCypher _rsa;
    
    private readonly IDialogRequestRepository _dialogReqeuestRepository;
    private readonly IDialogSecretRepository _dialogSecretRepository;
    private readonly IDialogRepository _dialogRepository;
    
    private readonly IUserRepository _userRepository;

    public DialogService(ISessionGetterService sessionGetterService,
        IRsaCypher rsa,
        IAesCypher aes,
        IDialogRequestRepository dialogRequestRepository,
        IDialogSecretRepository dialogSecretRepository,
        IDialogRepository dialogRepository,
        IUserRepository userRepository)
    {
        _sessionGetterService = sessionGetterService;
        _rsa = rsa;
        _aes = aes;
        _dialogReqeuestRepository = dialogRequestRepository;
        _userRepository = userRepository;
        _dialogReqeuestRepository = dialogRequestRepository;
        _dialogSecretRepository = dialogSecretRepository;
        _dialogRepository = dialogRepository;
    }

    public async Task<CreateDialogRequestResult> CreateDialogRequestAsync(Guid ownerUserId,
        Guid requestUserId,
        string ownerSessionId)
    {
        var result = new CreateDialogRequestResult();
        var savedDialogRequest = await _dialogReqeuestRepository.GetDialogRequestAsync(ownerUserId, requestUserId);
        if (savedDialogRequest != null)
        {
            result.SetError("Dialog request is alredy exist");
            return result;
        }

        var requestedUser = await _userRepository.GetUserByIdAsync(requestUserId);
        if (requestedUser == null)
        {
            Log.Error($"Cannot create dialog request, user #({requestedUser}) not exist");
            result.SetError("Cannot find user");
            return result;
        }

        var dialogRequestCreateResult = await _dialogReqeuestRepository.CreateDialogRequestAsync(ownerUserId, requestUserId);
        var createDialogResult =
            await _dialogRepository.CreateDialogAsync(ownerUserId, requestUserId, dialogRequestCreateResult);
        
        var aesKeys = _aes.GetAesKeyAndIv();
        await _dialogSecretRepository.CreateDialogSecretAsync(createDialogResult, aesKeys.key, aesKeys.iv);

        var session = await _sessionGetterService.GetSessionAsync(ownerUserId, Guid.Parse(ownerSessionId));
        if (session == null)
        {
            Log.Error($"Cannot find session #({ownerSessionId})");
            result.SetServerError();
            return result;
        }

        var cryptedAes = _rsa.Crypt(session.ClientPublicKey, aesKeys.key);
        var cryptedIv = _rsa.Crypt(session.ClientPublicKey, aesKeys.iv);
        
        result.SetSucessResult(dialogRequestCreateResult, createDialogResult, cryptedAes, cryptedIv);
        return result;
    }

    public async Task<GetDialogResult> GetDialogsAsync(Guid userId, string sessionId)
    {
        var result = new GetDialogResult();
        var dialogs = await _dialogRepository.GetUserDialogsAsync(userId);

        if (dialogs.Count == 0)
        {
            result.SetEmptySucessResult();
            return result;
        }
        
        var dialogIds = dialogs.Select(x => x.Id);
        var dialogSecrets = await _dialogSecretRepository.GetDialogSecretsAsync(dialogIds);
        var session = await _sessionGetterService.GetSessionAsync(userId, Guid.Parse(sessionId));
        if (session == null)
        {
            Log.Error($"Cannot find session. User #({userId}), Session #({sessionId})");
            return result;
        }

        var dialogResult = new List<GetDialogResult.Dialog>();
        foreach (var dialogSecret in dialogSecrets)
        {
            var cryptedKey = _rsa.Crypt(session.ClientPublicKey, dialogSecret.Value.key);
            var cryptedIV = _rsa.Crypt(session.ClientPublicKey, dialogSecret.Value.iv);
            var dialog = dialogs.FirstOrDefault(x => x.Id == dialogSecret.Key);
            var userName = dialog.User1Id != userId
                ? dialog.User1.UserName
                : dialog.User2.UserName;
            
            dialogResult.Add(new GetDialogResult.Dialog(dialogSecret.Key, userName, cryptedKey,
                cryptedIV, dialog.DialogRequest.IsAccepted));
        }
        
        result.SetSucessResult(dialogResult);
        return result;
    }
}