using Core.Repositories.Interfaces;
using Core.SessionServices.Services.Interfaces;
using Serilog;

namespace Core.SessionServices.Services.Implementations;

public class SessionCacheInitializerService : ISessionCacheInitializerService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IUserCypherKeyRepository _userCypherKeyRepository;

    private readonly ISessionCacheService _sessionCacheService;
    
    public SessionCacheInitializerService(ISessionRepository sessionRepository,
        IUserCypherKeyRepository userCypherKeyRepository,
        ISessionCacheService sessionCacheService)
    {
        _sessionRepository = sessionRepository;
        _userCypherKeyRepository = userCypherKeyRepository;
        _sessionCacheService = sessionCacheService;
    }

    public async Task SetSessionInCacheAsync(Guid sessionId,
        Guid userId)
    {
        var session = await _sessionRepository.GetSessionAsync(sessionId);
        if (session.UserId != userId)
        {
            Log.Error($"Session #({sessionId}) not for user: #({userId})");
            return;
        }

        var cypher = await _userCypherKeyRepository.GetCypherKeyBySessionIdAsync(sessionId);
        
        await _sessionCacheService.AddSessionInCacheAsync(userId, sessionId, session.ServerPublicKey,
            session.ServerPrivateKey, session.ClientPublicKey, cypher.CryptedKey, session.HmacKey); ;
    }
}