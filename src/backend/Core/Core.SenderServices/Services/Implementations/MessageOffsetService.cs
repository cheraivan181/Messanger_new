using System.Collections.Concurrent;
using Core.MessageServices.Services.Interfaces;
namespace Core.MessageServices.Services.Implementations;

public class MessageOffsetService : IMessageOffsetService
{
    private static readonly ConcurrentDictionary<Guid, Dictionary<string, int>> _cache =
        new ConcurrentDictionary<Guid, Dictionary<string, int>>();
    
    public void RegisterOffset(Guid userId, string connectionId)
    {
        _cache.TryAdd(userId, new Dictionary<string, int>()
        {
            {connectionId, 1}
        });
    }

    public void IncrementNotificationOffset(Guid userId)
    {
        if (!_cache.TryGetValue(userId, out var offsets))
        {
            throw new Exception("Cannot find offset");
        }

        foreach (var offset in offsets)
        {
            offsets[offset.Key] = offset.Value + 1;
        }
        
        
        _cache[userId] = offsets;
    }
    
    
    public void RemoveConnectionFromNotificationOffset(Guid userId, string connectionId)
    {
        if (!_cache.TryGetValue(userId, out var offsets))
        {
            throw new Exception("Cannot find notification offset");
        }

        offsets.Remove(connectionId);
        _cache[userId] = offsets;
    }
    
    
    public Dictionary<string, int> GetNotificationOffsets(Guid userId)
    {
        if (!_cache.TryGetValue(userId, out var result))
        {
            throw new Exception("Cannot find notification offset");
        }

        return result;
    }

    public int GetNotificationOffset(Guid userId, string connectionId)
    {
        if (!_cache.TryGetValue(userId, out var result))
        {
            throw new Exception("Cannot find notification offset");
        }

        if (!result.TryGetValue(connectionId, out var offset))
        {
            throw new Exception("Cannot find notification offset");
        }

        return offset;
    }
}