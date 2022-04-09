using Core.DbModels;

namespace Core.Repositories.Interfaces;

public interface ISessionRepository
{
    Task<Guid> CreateSessionAsync(Guid userId, string serverPrivateKey,
        string serverPublicKey, string clientPublicKey);

    Task<Session> GetSessionAsync(Guid sessionId);
}