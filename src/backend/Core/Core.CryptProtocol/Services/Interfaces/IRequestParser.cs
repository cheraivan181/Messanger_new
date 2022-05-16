using Core.BinarySerializer;
using Core.CryptProtocol.Domain;

namespace Core.CryptProtocol.Services.Interfaces;

public interface IRequestParser
{
    ParsedMessage ParseMessage(string hmacKey, string message);
    ParsedMessageModel ParseRequest(string hmacKey, string aesKey, string message);
}