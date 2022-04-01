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

    public async Task<ConnectionStoreModel> GetConnectionsFromCacheAsync(long userId)
    {
        var database = _databaseProvider.GetDatabase();
        var cacheKey = GetCacheKey(userId);
        
        var dataFromCache = await database.StringGetAsync(new RedisKey(cacheKey));
        var result = dataFromCache.FromJson<ConnectionStoreModel>();

        return result;
    }

    public async Task AddConnectionInCacheAsync(long userId, long sessionId, string connectionId)
    {
        var database = _databaseProvider.GetDatabase();
        var cacheKey = GetCacheKey(userId);

        var isKeyExist = await database.KeyExistsAsync(new RedisKey(cacheKey));
        if (isKeyExist)
        {
            var dataFromCache = await GetConnectionsFromCacheAsync(userId);
            dataFromCache.Connections.Add(new StoreModel(sessionId, connectionId));
            await database.StringSetAsync(cacheKey, new RedisValue(dataFromCache.ToJson()));
        }
        else
        {
            var connections = new ConnectionStoreModel();
            connections.Connections = new List<StoreModel>()
            {
                new StoreModel(sessionId, connectionId)
            };

            await database.StringSetAsync(cacheKey, new RedisValue(connections.ToJson()));
        }
    }

    public async Task RemoveConnectionsFromCacheAsync(long userId, string connectionId)
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
        var connections = dataFromCache.Connections.Where(x => x.connectionId != connectionId)
            .ToList();
        dataFromCache.Connections = connections;

        await database.StringSetAsync(cacheKey, new RedisValue(dataFromCache.ToJson()));
    }
    
    private string GetCacheKey(long userId)
    {
        return $"Connections-{userId}";
    }
}