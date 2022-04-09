using Core.Repositories.Interfaces;
using Core.SessionServices.Services.Interfaces;

namespace Core.SessionServices.Services.Implementations;

public class ConnectionCollectorService : IConnectionCollectorService
{
    private readonly IConnectionRepository _connectionRepository;
    private readonly IConnectionCollectorCacheService _connectionCollectorCacheService;
    
    public ConnectionCollectorService(IConnectionRepository connectionRepository,
        IConnectionCollectorCacheService connectionCollectorCacheService)
    {
        _connectionRepository = connectionRepository;
        _connectionCollectorCacheService = connectionCollectorCacheService;
    }
    
    public async Task AddConnectionAsync(Guid userId, Guid sessionId, string connectionId)
    {
        await _connectionRepository.AddConnectionAsync(userId, sessionId, connectionId);
        await _connectionCollectorCacheService.AddConnectionInCacheAsync(userId, sessionId, connectionId);
    }
    
    public async Task RemoveConnectionAsync(Guid userId, string connectionId)
    {
        await _connectionRepository.RemoveConnectionAsync(connectionId);
        await _connectionCollectorCacheService.RemoveConnectionsFromCacheAsync(userId, connectionId);
    }
}