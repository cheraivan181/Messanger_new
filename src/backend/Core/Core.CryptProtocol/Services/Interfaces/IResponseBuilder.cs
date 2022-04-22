using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Base.Responses;

namespace Core.CryptProtocol.Services.Interfaces;

public interface IResponseBuilder
{
    string BuildMessage<T>(T responseModel, ResponseCode statusCode,
        ResponseType responseType, string aesKey, string hmacSignKey,
        string errorMessage = null) where T : class, ISerializableMessage;
}