using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Base.Responses;

namespace Core.CryptProtocol.Services.Interfaces;

public interface IResponseBuilder
{
    string BuildMessage<T>(T responseModel, ResponseCode statusCode,
        ResponseAction responseAction, string aesKey, string hmacSignKey,
        int notificationOffset) where T : class, ISerializableMessage;
}