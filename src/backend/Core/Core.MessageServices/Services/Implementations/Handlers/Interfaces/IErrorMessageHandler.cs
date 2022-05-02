namespace Core.MessageServices.Services.Implementations.Handlers.Interfaces;

public interface IErrorMessageHandler
{
    Task HandleAsync(Guid actionId, Guid userId, string connectionId, string text);
}