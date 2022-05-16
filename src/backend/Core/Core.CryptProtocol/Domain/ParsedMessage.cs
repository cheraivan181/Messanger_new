using Core.CryptProtocol.Domain.Base.Responses;

namespace Core.CryptProtocol.Domain;

public class ParsedMessage
{
    public RequestProtocolMessage RequestProtocolMessage { get; set; }
    public bool IsSucess;
}