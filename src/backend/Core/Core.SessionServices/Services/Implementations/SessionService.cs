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
    private readonly IUserCypherKeyRepository _userCypherKeyRepository;
    
    private readonly ISessionCacheService _sessionCacheService;
    
    private readonly IRsaCypher _rsaCypher;
    private readonly IAesCypher _aes;
    
    public SessionService(IUserRoleRepository userRoleRepository,
        IRoleRepository roleRepository,
        ISessionRepository sessionRepository,
        IUserCypherKeyRepository userCypherKeyRepository,
        IRsaCypher rsaCypher,
        IAesCypher aes,
        ISessionCacheService sessionCacheService)
    {
        _userRoleRepository = userRoleRepository;
        _roleRepository = roleRepository;
        _sessionRepository = sessionRepository;
        _rsaCypher = rsaCypher;
        _aes = aes;
        _sessionCacheService = sessionCacheService;
        _userCypherKeyRepository = userCypherKeyRepository;
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
        var hmacKey = CryptoRandomizer.GetRandomString(16);
        
        Guid createdSessionId;

        try
        {
            createdSessionId = await _sessionRepository.CreateSessionAsync(userId, serverKeys.privateKey,
                serverKeys.publicKey, clientPublicKey, hmacKey);
        }
        catch (Exception ex)
        {
            Log.Error($"Cannot create session. UserId: #({userId})", ex);
            result.SetInternalServerError();
            return result;
        }
        
        var aes = _aes.GetAesKeyAndIv();
        Guid createdCypherKey;

        try
        {
            createdCypherKey = await _userCypherKeyRepository.CreateAsync(createdSessionId, aes.key);
        }
        catch (Exception ex)
        {
            Log.Error($"Cannot create cypher key, session #({createdSessionId})", ex);
            result.SetInternalServerError();;
            return result;
        };
        
        
        Log.Debug($"Session was created for user #({userId})");
        
        await _sessionCacheService.AddSessionInCacheAsync(userId, createdSessionId, serverKeys.publicKey,
            serverKeys.privateKey, clientPublicKey, aes.key, hmacKey);
        
        Log.Debug($"Session #({createdSessionId}) was added to cache");

        var cryptedAes = _rsaCypher.Crypt(clientPublicKey, aes.key);
        result.SetSucessResult(serverKeys.publicKey, createdSessionId,hmacKey, cryptedAes, isNeedUpdateToken);
        return result;
    }
}