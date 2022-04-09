using Core.CryptService.Interfaces;
using Core.DbModels;
using Core.Repositories.Interfaces;
using Core.SessionServices.Domain;
using Core.SessionServices.Services.Interfaces;
using Core.Utils;
using Serilog;

namespace Core.SessionServices.Services.Implementations;

public class SessionService : ISessionService
{
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ISessionRepository _sessionRepository;
    
    private readonly ISessionCacheService _sessionCacheService;
    
    private readonly IRsaCypher _rsaCypher;
    
    public SessionService(IUserRoleRepository userRoleRepository,
        IRoleRepository roleRepository,
        ISessionRepository sessionRepository,
        IRsaCypher rsaCypher,
        ISessionCacheService sessionCacheService)
    {
        _userRoleRepository = userRoleRepository;
        _roleRepository = roleRepository;
        _sessionRepository = sessionRepository;
        _rsaCypher = rsaCypher;
        _sessionCacheService = sessionCacheService;
    }
    
    public async Task<CreateSessionResponse> CreateSessionAsync(Guid userId,
        string clientPublicKey)
    {
        Log.Debug($"User #({userId}) try to create session");
        
        var result = new CreateSessionResponse();
        var userRoles = await _userRoleRepository.GetUserRolesAsync(userId);
        var isNeedUpdateToken = false;
        if (!userRoles.Any(x => x.Name == CommonConstants.ProtocoledUser))
        {
            var role = await _roleRepository.GetRoleByNameAsync(CommonConstants.ProtocoledUser);
            if (role == null)
            {
                result.SetInternalServerError();
                return result;
            }
            
            await _userRoleRepository.AddUserRoleAsync(userId, role.Id);
            Log.Debug($"user #({userId}) add to UserRole");

            isNeedUpdateToken = true;
        }
 
        var serverKeys = _rsaCypher.GenerateKeys();
        var session = new Session()
        {
            UserId = userId,
            ServerPrivateKey = serverKeys.privateKey,
            ServerPublicKey = serverKeys.publicKey,
            ClientPublicKey = clientPublicKey
        };

        Guid createdSessionId;

        try
        {
            createdSessionId = await _sessionRepository.CreateSessionAsync(userId, serverKeys.privateKey,
                serverKeys.publicKey, clientPublicKey);
        }
        catch (Exception ex)
        {
            Log.Error($"Cannot create session. UserId: #({userId})", ex);
            result.SetInternalServerError();
            return result;
        }

        Log.Debug($"Session was created for user #({userId})");
        
        await _sessionCacheService.AddSessionInCacheAsync(userId, createdSessionId, serverKeys.publicKey,
            serverKeys.privateKey, clientPublicKey);
        
        Log.Debug($"Session #({createdSessionId}) was added to cache");
        
        result.SetSucessResult(serverKeys.publicKey, createdSessionId, isNeedUpdateToken);
        return result;
    }

    public async Task AddSessionInCacheAsync(Guid userId, Guid sessionId)
    {
        var sessionFromCache = await _sessionCacheService.GetSessionAsync(userId, sessionId);
        if (sessionFromCache != null)
        {
            Log.Warning($"Try to add #({sessionId}) in cache, but it is in cache alredy");
            return;
        }

        var sessionFromDb = await _sessionRepository.GetSessionAsync(sessionId);
        if (sessionFromDb == null)
        {
            Log.Error($"Error! Cannot get session from database #({sessionId})");
            return;
        }

        await _sessionCacheService.AddSessionInCacheAsync(userId, sessionId, sessionFromDb.ServerPublicKey,
            sessionFromDb.ServerPrivateKey, sessionFromDb.ClientPublicKey);
    }
}