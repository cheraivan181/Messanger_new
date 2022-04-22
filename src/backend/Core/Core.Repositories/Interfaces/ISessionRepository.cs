using Core.DbModels;

namespace Core.Repositories.Interfaces;

public interface ISessionRepository
{
    Task<Guid> CreateSessionAsync(Guid userId, string serverPrivateKey,
        string serverPublicKey, string clientPublicKey, string hmacKey);

    Task<Session> GetSessionAsync(Guid sessionId);
}