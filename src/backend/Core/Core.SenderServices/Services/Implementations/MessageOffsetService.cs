using Core.CacheServices.Interfaces.Base;
using Core.MessageServices.Services.Interfaces;
using Core.Utils;
using StackExchange.Redis;

namespace Core.MessageServices.Services.Implementations;


public class MessageOffsetService : IMessageOffsetService
{
    private readonly IDatabaseProvider _databaseProvider;

    public MessageOffsetService(IDatabaseProvider databaseProvider)
    {
        _databaseProvider = databaseProvider;
    }

    public async Task RegisterOffsetAsync(Guid userId, string connectionId)
    {
        var database = _databaseProvider.GetDatabase();
        var key = GetCacheKey(userId);
        await database.HashSetAsync(key, new HashEntry[]
        {
            new HashEntry(connectionId, 1)
        });
    }

    public async Task IncrementNotificationOffsetAsync(Guid userId)
    {
        var database = _databaseProvider.GetDatabase();
        var key = GetCacheKey(userId);
        
        var connectionIds = await database.HashKeysAsync(key);
        var batch = database.CreateBatch();

        foreach (var currentConnectionId in connectionIds)
        {
            batch.HashIncrementAsync(key.ToRedisKey(), currentConnectionId);
        }
        
        batch.Execute();
    }

    public async Task RemoveConnectionFromNotificationOffsetAsync(Guid userId, string connectionId)
    {
        var database = _databaseProvider.GetDatabase();
        var key = GetCacheKey(userId);

        await database.HashDeleteAsync(key, connectionId);
        var lenght = await database.HashLengthAsync(key);
        if (lenght == 0)
            await database.KeyDeleteAsync(key);
    }

    public async Task<Dictionary<string, int>> GetNotificationOffsetsAsync(Guid userId)
    {
        var result = new Dictionary<string, int>();
        var database = _databaseProvider.GetDatabase();
        var key = GetCacheKey(userId);

        var hash = await database.HashGetAllAsync(key);
        foreach (var item in hash)
            result.Add(item.Key.ToString(), (int)item.Value);
        
        return result;
    }
    
    private string GetCacheKey(Guid userId) =>
        $"offset-{userId}";
}