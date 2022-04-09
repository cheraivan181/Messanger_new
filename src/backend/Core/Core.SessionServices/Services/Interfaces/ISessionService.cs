using Core.SessionServices.Domain;

namespace Core.SessionServices.Services.Interfaces;

public interface ISessionService
{
    Task<CreateSessionResponse> CreateSessionAsync(Guid userId,
        string clientPublicKey);

    Task AddSessionInCacheAsync(Guid userId, Guid sessionId);
}