using Core.SessionServices.Domain;

namespace Core.SessionServices.Services.Interfaces;

public interface IConnectionCollectorCacheService
{
    Task<ConnectionStoreModel> GetConnectionsFromCacheAsync(long userId);
    Task AddConnectionInCacheAsync(long userId, long sessionId, string connectionId);
    Task RemoveConnectionsFromCacheAsync(long userId, string connectionId);
}