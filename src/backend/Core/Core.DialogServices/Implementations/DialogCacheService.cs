using Core.BinarySerializer;
using Core.CacheServices.Interfaces.Base;
using Core.DbModels;
using Core.DialogServices.Domain;
using Core.DialogServices.Interfaces;
using Core.Utils;
using StackExchange.Redis;

namespace Core.DialogServices.Implementations;

public class DialogCacheService : IDialogCacheService
{
    private readonly IDatabaseProvider _databaseProvider;

    public DialogCacheService(IDatabaseProvider databaseProvider)
    {
        _databaseProvider = databaseProvider;
    }

    public async Task<List<DialogCacheModel>> GetDialogsFromCacheAsync(Guid userId)
    {
        var database = _databaseProvider.GetDatabase();
        var cacheKey = GetCacheKey(userId);

        var dialogs = await database.ListRangeAsync(cacheKey, 0, -1);
        return dialogs.Select(x => x.ToString().FromBinaryMessage<DialogCacheModel>())
            .ToList();
    }
    
    public void SetDialogsInCache(Guid userId, List<Dialog> dialogs)
    {
        var database = _databaseProvider.GetDatabase();
        var cacheKey = GetCacheKey(userId);
        var batch = database.CreateBatch();
        
        batch.KeyExpireAsync(cacheKey, TimeSpan.FromMinutes(CommonConstants.DialogListCache), CommandFlags.FireAndForget);
        
        var dialogCacheModel = new DialogCacheModel();
        foreach (var dialog in dialogs)
        {
            dialogCacheModel.DialogId = dialog.Id;
            batch.ListRightPushAsync(cacheKey, dialogCacheModel.ToBinaryMessage());
        }
        
        batch.Execute();
    }

    public async Task AddDialogInCacheAsync(Guid user1Id, Guid user2Id, Guid dialogId)
    {
        var database = _databaseProvider.GetDatabase();
        var cacheKey = GetCacheKey(user1Id);

        var newDialogId = new DialogCacheModel()
        {
            DialogId = dialogId
        };

        var binaryMessage = newDialogId.ToBinaryMessage();
        
        var tasks = new List<Task>();
        tasks.Add(Task.Run(() => database.ListRightPushAsync(GetCacheKey(user1Id), binaryMessage)));
        tasks.Add(Task.Run(() => database.ListRightPushAsync(GetCacheKey(user2Id), binaryMessage)));

        await Task.WhenAll(tasks);
    }
    
    private string GetCacheKey(Guid dialogId)
    {
        return $"DialogList-{dialogId}";
    }
}