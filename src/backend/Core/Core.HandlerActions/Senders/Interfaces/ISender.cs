using Core.CryptProtocol.Domain.Base.Responses;
using Core.CryptProtocol.Domain.Interfaces;

namespace Core.MessageServices.Services.Implementations.Senders.Interfaces;

public interface ISender
{
    ResponseAction ResponseAction { get; }
    
    int ProtocolVersion { get; }
    
    Task SendAsync(Guid userId, IResponseProtocolMessage responseProtocolMessage, string connectionId = null);
}