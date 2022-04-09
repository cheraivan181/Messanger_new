using Core.SessionServices.Domain;

namespace Core.SessionServices.Services.Interfaces;

public interface IConnectionCollectorCacheService
{
    Task<ConnectionStoreModel> GetConnectionsFromCacheAsync(Guid userId);
    Task AddConnectionInCacheAsync(Guid userId, Guid sessionId, string connectionId);
    Task RemoveConnectionsFromCacheAsync(Guid userId, string connectionId);
}