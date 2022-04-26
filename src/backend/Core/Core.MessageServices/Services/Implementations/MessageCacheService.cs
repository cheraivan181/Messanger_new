using Core.CacheServices.Interfaces.Base;
using Core.MessageServices.Services.Interfaces;

namespace Core.MessageServices.Services.Implementations;

public class MessageCacheService : IMessageCacheService
{
    private readonly IDatabaseProvider _databaseProvider;
    
    public MessageCacheService(IDatabaseProvider databaseProvider)
    {
        _databaseProvider = databaseProvider;
    }

    public async Task InitializeCacheAsync(Guid userId, Guid dialogId)
    {
        var db = _databaseProvider.GetDatabase();    
    }

    private string GetCacheKey(Guid dialogId) =>
        $"messagecache-{dialogId}";
}