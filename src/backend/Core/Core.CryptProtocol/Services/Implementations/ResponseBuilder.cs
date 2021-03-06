using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Base.Responses;
using Core.CryptProtocol.Services.Interfaces;
using Core.CryptService.Interfaces;

namespace Core.CryptProtocol.Services.Implementations;

public class ResponseBuilder : IResponseBuilder
{
    private readonly IAesCypher _aes;
    private readonly IHmacService _hmac;

    public ResponseBuilder(IAesCypher aes,
        IHmacService hmac)
    {
        _aes = aes;
        _hmac = hmac;
    }
    
    public string BuildMessage<T>(T responseModel, ResponseCode statusCode,
        ResponseAction responseAction,string aesKey, string hmacSignKey, 
        int notificationOffset) where T:class, ISerializableMessage
    {
        var response = new ResponseProtocolMessage();
        var iv = _aes.GetAesKeyAndIv().iv;

        var binaryResponseModel = responseModel.ToBinaryMessage();
        var cryptedMessage = _aes.Crypt(aesKey, iv, binaryResponseModel);
        var sign = _hmac.GetSignature(hmacSignKey, cryptedMessage);

        response.Sign = sign;
        response.IV = iv;
        response.PayLoad = cryptedMessage;
        response.ResponseAction = responseAction;
        response.NotificationOffset = notificationOffset;
        response.ResponseCode = statusCode;

        var binaryResponse = response.ToBinaryMessage();
        return binaryResponse;
    }
}