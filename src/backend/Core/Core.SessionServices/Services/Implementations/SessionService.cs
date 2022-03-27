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
    private readonly ISessionCacheService _sessionCacheService;
    
    private readonly IRsaCypher _rsaCypher;
    
    public SessionService(IUserRoleRepository userRoleRepository,
        IRoleRepository roleRepository,
        IRsaCypher rsaCypher,
        ISessionCacheService sessionCacheService)
    {
        _userRoleRepository = userRoleRepository;
        _roleRepository = roleRepository;
        _rsaCypher = rsaCypher;
        _sessionCacheService = sessionCacheService;
    }
    
    public async Task<CreateSessionResponse> CreateSessionAsync(long userId,
        string clientPublicKey)
    {
        var result = new CreateSessionResponse();
        var userRoles = await _userRoleRepository.GetUserRolesAsync(userId);
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
        }
 
        var serverKeys = _rsaCypher.GenerateKeys();
        var session = new Session()
        {
            UserId = userId,
            ServerPrivateKey = serverKeys.privateKey,
            ServerPublicKey = serverKeys.publicKey,
            ClientPublicKey = clientPublicKey
        };
        
        result.SetSucessResult();
        return result;
    }
}