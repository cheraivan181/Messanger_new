namespace ConnectionHandler.Services.Interfaces;

public interface IRedisBusService
{
    Task SubscribeToChanel(Guid userId, string connectionId);
    Task UnsubscribeChanel(Guid userId, string channel);
}