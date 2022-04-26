namespace Core.MessageServices.Services.Interfaces;

public interface IMessageCacheInitializerService
{
    Task InitializeMessageCacheAsync(Guid userId);
    Task RemoveMessagesFromCacheAsync(Guid userId);
}