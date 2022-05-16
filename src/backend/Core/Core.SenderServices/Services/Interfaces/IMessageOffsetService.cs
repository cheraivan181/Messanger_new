namespace Core.MessageServices.Services.Interfaces;

public interface IMessageOffsetService
{
    Dictionary<string, int> GetNotificationOffsets(Guid userId);
    void RegisterOffset(Guid userId, string connectionId);
    void IncrementNotificationOffset(Guid userId);
    void RemoveConnectionFromNotificationOffset(Guid userId, string connectionId);
    int GetNotificationOffset(Guid userId, string connectionId);
}