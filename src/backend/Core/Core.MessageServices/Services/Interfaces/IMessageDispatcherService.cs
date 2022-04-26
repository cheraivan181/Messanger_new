using Core.MessageServices.Domain;

namespace Core.MessageServices.Services.Interfaces;

public interface IMessageDispatcherService
{
    Task DispatchMessage(Guid userId,
        Guid sessionId,
        string message,
        MessageType messageType);
}