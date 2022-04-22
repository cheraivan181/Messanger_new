using Core.BinarySerializer;
using Core.CryptProtocol.Domain;

namespace Core.CryptProtocol.Services.Interfaces;

public interface IRequestParser
{
    ParsedResult<T> ParseRequest<T>(string hmacKey, string aesKey,
        string iv, string message) where T : class, ISerializableMessage, new();
}