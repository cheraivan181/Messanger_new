using Core.SessionServices.Domain;

namespace Core.SessionServices.Services.Interfaces;

public interface ISessionCacheService
{
    Task AddSessionInCacheAsync(long userId, long sessionId, string serverPublicKey,
        string serverPrivateKey, string clientPublicKey);

    Task<List<SessionModel>> GetSessionsAsync(long userId);
    Task<SessionModel> GetSessionAsync(long userId, long sessionId);
}