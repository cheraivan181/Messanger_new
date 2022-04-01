using Core.DbModels;

namespace Core.Repositories.Interfaces;

public interface ISessionRepository
{
    Task<long> CreateSessionAsync(long userId, string serverPrivateKey,
        string serverPublicKey, string clientPublicKey);

    Task<Session> GetSessionAsync(long sessionId);
}