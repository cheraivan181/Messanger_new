using Core.CryptProtocol.Domain;
using Core.MessageServices.Domain;

namespace Core.MessageServices.Services.Interfaces;

public interface IMessageRouterService
{
    Task DispatchMessageAsync(DispatchMessageRequest request);
}