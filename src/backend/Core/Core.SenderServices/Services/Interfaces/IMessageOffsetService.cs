namespace Core.MessageServices.Services.Interfaces;

public interface IMessageOffsetService
{
    Task<Dictionary<string, int>> GetNotificationOffsetsAsync(Guid userId);
    Task RegisterOffsetAsync(Guid userId, string connectionId);
    Task IncrementNotificationOffsetAsync(Guid userId);
    Task RemoveConnectionFromNotificationOffsetAsync(Guid userId, string connectionId);
}