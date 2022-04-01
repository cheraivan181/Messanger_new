using Core.SessionServices.Domain;

namespace Core.SessionServices.Services.Interfaces;

public interface ISessionService
{
    Task<CreateSessionResponse> CreateSessionAsync(long userId,
        string clientPublicKey);

    Task AddSessionInCacheAsync(long userId, long sessionId);
}