using Core.CacheServices.Interfaces.Base;
using Core.SessionServices.Domain;
using Core.SessionServices.Services.Interfaces;
using Core.Utils;
using StackExchange.Redis;

namespace Core.SessionServices.Services.Implementations;

public sealed class SessionCacheService : ISessionCacheService
{
    private readonly IDatabaseProvider _databaseProvider;
    
    public SessionCacheService(IDatabaseProvider databaseProvider)
    {
        _databaseProvider = databaseProvider;
    }
    
    public async Task AddSessionInCacheAsync(long userId, long sessionId, string serverPublicKey,
        string serverPrivateKey, string clientPublicKey)
    {
        var database = _databaseProvider.GetDatabase();
        var cacheKey = GetSessionCacheKey(userId);
        var cacheModel = new SessionModel(sessionId, serverPrivateKey, serverPublicKey, clientPublicKey)
            .ToJson();
        
        // продумать логику удаления старых данных...
        
        await database.ListRightPushAsync(new RedisKey(cacheKey), new RedisValue(cacheModel));
    }
        
    public async Task<List<SessionModel>> GetSessionsAsync(long userId)
    { 
        var database = _databaseProvider.GetDatabase();
        var cacheKey = GetSessionCacheKey(userId);
        var lenght = await database.ListLengthAsync(new RedisKey(cacheKey));
        if (lenght == 0)
            return new List<SessionModel>();

        var list = await database.ListRangeAsync(new RedisKey(cacheKey), 0, lenght - 1);
        var result = list.Select(x => x.FromJson<SessionModel>())
            .ToList();
        
        return result;
    }

    public async Task<SessionModel> GetSessionAsync(long userId, long sessionId)
    {
        var cacheModels = await GetSessionsAsync(userId);
        return cacheModels.FirstOrDefault(x => x.SessionId == sessionId);
    }
    
    private string GetSessionCacheKey(long userId)
    {
        return $"session-{userId}";
    }
}