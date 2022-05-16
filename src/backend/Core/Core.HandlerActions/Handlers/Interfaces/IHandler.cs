using Core.CryptProtocol.Domain.Base;
using Core.CryptProtocol.Domain.Interfaces;

namespace Core.MessageServices.Services.Implementations.Handlers.Interfaces;

public interface IHandler
{
    RequestType RequestType { get; }
    
    int ProtocolVersion { get; }
    
    Task HandleAsync(IRequestProtocolMessage requestProtocolMessage);
}