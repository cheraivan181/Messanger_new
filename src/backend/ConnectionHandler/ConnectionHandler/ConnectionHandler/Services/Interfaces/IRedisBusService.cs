namespace ConnectionHandler.Services.Interfaces;

public interface IRedisBusService
{
    Task SubscribeToChanel(Guid userId, string connectionId);
}