using Core.BinarySerializer;
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
    
    public async Task AddSessionInCacheAsync(Guid userId, Guid sessionId, string serverPublicKey,
        string serverPrivateKey, string clientPublicKey, string aes, string hmacKey)
    {
        var database = _databaseProvider.GetDatabase();
        var cacheKey = GetSessionCacheKey(userId);

        if (!await database.KeyExistsAsync(cacheKey))
        {
            database.KeyExpire(cacheKey, TimeSpan.FromMinutes(CommonConstants.MinutesSessionInCache), 
                CommandFlags.FireAndForget);
        } 
        //todo:: lua
        
        var cacheModel = new SessionModel(sessionId, serverPrivateKey, serverPublicKey, clientPublicKey, aes, hmacKey)
            .ToBinaryMessage();
        
        await database.ListRightPushAsync(new RedisKey(cacheKey), new RedisValue(cacheModel));
    }

    public async Task RemoveSessionFromCacheAsync(Guid userId, Guid sessionId)
    {
        var database = _databaseProvider.GetDatabase();
        var cacheKey = GetSessionCacheKey(userId);

        var lstLength = await database.ListLengthAsync(cacheKey);
        
        if (lstLength == 1)
        {
            database.KeyDelete(cacheKey, CommandFlags.FireAndForget);
            return;
        }
        
        var sessions = await database.ListRangeAsync(cacheKey, 0, -1);
        for (int i = 0; i < sessions.Length; i++)
        {
            if (sessions[i].FromJson<SessionModel>().SessionId == sessionId)
            {
                await database.ListRemoveAsync(cacheKey.ToRedisKey(), sessions[i]);
                break;
            }
        }
    }
        
    public async Task<List<SessionModel>> GetSessionsAsync(Guid userId)
    { 
        var database = _databaseProvider.GetDatabase();
        var cacheKey = GetSessionCacheKey(userId);

        var list = await database.ListRangeAsync(new RedisKey(cacheKey), 0, -1);
        var result = list.Select(x => x.ToString().FromBinaryMessage<SessionModel>())
            .ToList();
        
        return result;
    }
    
    public async Task<SessionModel> GetSessionAsync(Guid userId, Guid sessionId)
    {
        var cacheModels = await GetSessionsAsync(userId);
        return cacheModels.FirstOrDefault(x => x.SessionId == sessionId);
    }
    
    private string GetSessionCacheKey(Guid userId)
    {
        return $"session-{userId}";
    }
}