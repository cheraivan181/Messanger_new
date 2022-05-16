using Core.DbModels;

namespace Core.Repositories.Interfaces;

public interface IUserCypherKeyRepository
{
    Task<Guid> CreateAsync(Guid sessionId, string key);
    Task<UserCypherKey> GetCypherKeyBySessionIdAsync(Guid sessionId);
}