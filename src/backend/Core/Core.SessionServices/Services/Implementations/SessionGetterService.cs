using Core.Repositories.Interfaces;
using Core.SessionServices.Domain;
using Core.SessionServices.Services.Interfaces;
using Serilog;

namespace Core.SessionServices.Services.Implementations;

public class SessionGetterService : ISessionGetterService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IUserCypherKeyRepository _userCypherKeyRepository;
    private readonly ISessionCacheService _sessionCacheService;

    public SessionGetterService(ISessionRepository sessionRepository,
        IUserCypherKeyRepository userCypherKeyRepository,
        ISessionCacheService sessionCacheService)
    {
        _sessionRepository = sessionRepository;
        _userCypherKeyRepository = userCypherKeyRepository;
        _sessionCacheService = sessionCacheService;
    }

    public async Task<SessionModel> GetSessionAsync(Guid userId, Guid sessionId)
    {
        var sessionsFromCache = await _sessionCacheService.GetSessionAsync(userId, sessionId);
        if (sessionsFromCache == null)
        {
            Log.Debug($"Cannot get session #({sessionId}) from cache. Try to get session from db");
            var sessionFromDb = await _sessionRepository.GetSessionAsync(sessionId);
            if (sessionFromDb == null)
            {
                Log.Debug($"Cannot get session #({sessionId}) from db");
                return null;
            }

            var cypher = await _userCypherKeyRepository.GetCypherKeyBySessionIdAsync(sessionId);
            
            Log.Debug($"Set session #({sessionId}) in cache");
            await _sessionCacheService.AddSessionInCacheAsync(userId, sessionId, sessionFromDb.ServerPrivateKey,
                sessionFromDb.ServerPublicKey, sessionFromDb.ClientPublicKey, cypher.CryptedKey, 
                sessionFromDb.HmacKey);
            
            sessionsFromCache = new SessionModel(sessionId, sessionFromDb.ServerPrivateKey,
                sessionFromDb.ServerPublicKey, sessionFromDb.ClientPublicKey, cypher.CryptedKey, sessionFromDb.HmacKey);
            
            return sessionsFromCache;
        }

        return sessionsFromCache;
    }

    public async Task<List<SessionModel>> GetSessionsAsync(Guid userId)
    {
        var sessionsFromCache = await _sessionCacheService.GetSessionsAsync(userId);
        return sessionsFromCache;
    }
}