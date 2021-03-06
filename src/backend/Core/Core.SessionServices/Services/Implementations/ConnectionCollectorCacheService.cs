using Core.BinarySerializer;
using Core.CacheServices.Interfaces.Base;
using Core.SessionServices.Domain;
using Core.SessionServices.Services.Interfaces;
using Core.Utils;
using Serilog;
using StackExchange.Redis;

namespace Core.SessionServices.Services.Implementations;

public class ConnectionCollectorCacheService : IConnectionCollectorCacheService
{
    private readonly IDatabaseProvider _databaseProvider;

    public ConnectionCollectorCacheService(IDatabaseProvider databaseProvider)
    {
        _databaseProvider = databaseProvider;
    }

    public async Task<ConnectionStoreModel> GetConnectionsFromCacheAsync(Guid userId)
    {
        var database = _databaseProvider.GetDatabase();
        var cacheKey = GetCacheKey(userId);
        
        var dataFromCache = await database.StringGetAsync(new RedisKey(cacheKey));
        var result = dataFromCache.FromJson<ConnectionStoreModel>();

        return result;
    }

    public async Task AddConnectionInCacheAsync(Guid userId, Guid sessionId, string connectionId)
    {
        var database = _databaseProvider.GetDatabase();
        var cacheKey = GetCacheKey(userId);

        var isKeyExist = await database.KeyExistsAsync(new RedisKey(cacheKey));
        if (isKeyExist)
        {
            var dataFromCache = await GetConnectionsFromCacheAsync(userId);
            dataFromCache.Connections.Add(new ConnectionInCacheModel(sessionId, connectionId));
            await database.StringSetAsync(cacheKey, new RedisValue(dataFromCache.ToBinaryMessage()), TimeSpan.FromMinutes(CommonConstants.MinutesConnectionsInCache));
        }
        else
        {
            var connections = new ConnectionStoreModel();
            connections.Connections = new List<ConnectionInCacheModel>()
            {
                new ConnectionInCacheModel(sessionId, connectionId)
            };

            await database.StringSetAsync(cacheKey, new RedisValue(connections.ToJson()), TimeSpan.FromMinutes(CommonConstants.MinutesConnectionsInCache));
        }
    }

    public async Task RemoveConnectionsFromCacheAsync(Guid userId, string connectionId)
    {
        var database = _databaseProvider.GetDatabase();
        var cacheKey = new RedisKey(GetCacheKey(userId));
        var isDataExist = await database.KeyExistsAsync(cacheKey);

        if (!isDataExist)  
        {
            Log.Error($"Cannot find connection #({connectionId}), cacheKey: #({cacheKey})");
            return;
        }

        var dataFromCache = (await database.StringGetAsync(cacheKey))
            .FromJson<ConnectionStoreModel>();
        var connections = dataFromCache.Connections.Where(x => x.ConnectionId != connectionId)
            .ToList();

        if (connections.Count == 0)
        {
            database.KeyDelete(cacheKey, CommandFlags.FireAndForget);
            return;
        }
        
        dataFromCache.Connections = connections;

        await database.StringSetAsync(cacheKey, new RedisValue(dataFromCache.ToJson()));
    }
    
    private string GetCacheKey(Guid userId)
    {
        return $"Connections-{userId}";
    }
}