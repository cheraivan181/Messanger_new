using Core.CryptProtocol.Domain;

namespace Core.MessageServices.Services.Implementations.Handlers.Interfaces;

public interface IGetDialogMessageHandler
{
    Task HandleAsync(GetMessageRequest request);
}