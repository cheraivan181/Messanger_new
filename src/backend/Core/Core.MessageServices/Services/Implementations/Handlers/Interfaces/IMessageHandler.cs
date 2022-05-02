using Core.CryptProtocol.Domain;

namespace Core.MessageServices.Services.Implementations.Handlers.Interfaces;

public interface IDirectMessageHandler
{
    Task HandleAsync(SendDirectMessageRequest request);
}