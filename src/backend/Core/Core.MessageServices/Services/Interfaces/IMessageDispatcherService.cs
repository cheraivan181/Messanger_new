using Core.CryptProtocol.Domain;
using Core.MessageServices.Domain;

namespace Core.MessageServices.Services.Interfaces;

public interface IMessageDispatcherService
{
    Task DispatchMessage(DispatchMessageRequest request);
}