using Core.SessionServices.Domain;

namespace Core.SessionServices.Services.Interfaces;

public interface ISessionCacheService
{
    Task AddSessionInCacheAsync(Guid userId, Guid sessionId, string serverPublicKey,
        string serverPrivateKey, string clientPublicKey, string hmacKey);

    Task<List<SessionModel>> GetSessionsAsync(Guid userId);
    Task<SessionModel> GetSessionAsync(Guid userId, Guid sessionId);
    Task RemoveSessonFromCacheAsync(Guid userId, Guid sessionId);
}