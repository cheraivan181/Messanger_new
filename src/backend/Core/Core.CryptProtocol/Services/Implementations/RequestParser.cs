using Core.BinarySerializer;
using Core.CryptProtocol.Domain;
using Core.CryptProtocol.Domain.Base.Responses;
using Core.CryptProtocol.Services.Interfaces;
using Core.CryptService.Interfaces;
using Core.Utils;
using Serilog;

namespace Core.CryptProtocol.Services.Implementations;

public class RequestParser : IRequestParser
{
    private readonly IHmacService _hmacService;
    private readonly IAesCypher _aes;

    public RequestParser(IHmacService hmacService,
        IAesCypher aes)
    {
        _hmacService = hmacService;
        _aes = aes;
    }
    
    public ParsedResult<T> ParseRequest<T>(string hmacKey,string aesKey, 
        string iv, string message) where T:class, ISerializableMessage, new()
    {
        var result = new ParsedResult<T>();
        
        Message messageModel;
        try
        {
            messageModel = message.FromBinaryMessage<Message>();
        }
        catch (Exception ex)
        {
            result.ResponseCode = ResponseCode.CannotDeserializeRequestMessage;
            Log.Error($"Cannot deserialize message #({message})", ex);
        
            return result;
        }
        
        var sign = _hmacService.GetSignature(hmacKey, messageModel.Payload);
        if (sign != messageModel.Sign)
        {
            Log.Warning($"Warning!!! Try to hack protocol!!! #({message})");
            result.ResponseCode = ResponseCode.InvalidSign;

            return result;
        }

        string decryptedPayload;

        try
        {
            decryptedPayload = _aes.Decrypt(messageModel.Payload, aesKey, iv);
        }
        catch (Exception ex)
        {
            Log.Error($"Cannot decrypt message #({messageModel.Payload})", ex);
            result.ResponseCode = ResponseCode.InvalidCryptText;
            return result;
        }

        var requestModel = decryptedPayload.FromBinaryMessage<T>();
        result.ResponseCode = ResponseCode.Sucess;

        return result;
    }
}