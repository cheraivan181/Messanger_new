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

    public ParsedMessage ParseMessage(string hmacKey, string message)
    {
        var result = new ParsedMessage();
        RequestProtocolMessage requestProtocolMessageModel;
        try
        {
            requestProtocolMessageModel = message.FromBinaryMessage<RequestProtocolMessage>();
        }
        catch (Exception ex)
        {
            Log.Error($"Cannot deserialize message #({message})", ex);
            return result;
        }
        
        var sign = _hmacService.GetSignature(hmacKey, requestProtocolMessageModel.Payload);
        if (sign != requestProtocolMessageModel.Sign)
        {
            Log.Warning($"Warning!!! Try to hack protocol!!! #({message})");
            return result;
        }

        result.IsSucess = true;
        return result;
    }
    
    public ParsedMessageModel ParseRequest(string hmacKey,string aesKey, string message)
    {
        var result = new ParsedMessageModel();
        
        RequestProtocolMessage requestProtocolMessageModel;
        try
        {
            requestProtocolMessageModel = message.FromBinaryMessage<RequestProtocolMessage>();
        }
        catch (Exception ex)
        {
            Log.Error($"Cannot deserialize message #({message})", ex);
            return result;
        }
        
        var sign = _hmacService.GetSignature(hmacKey, requestProtocolMessageModel.Payload);
        if (sign != requestProtocolMessageModel.Sign)
        {
            Log.Warning($"Warning!!! Try to hack protocol!!! #({message})");
            return result;
        }
        
        try
        {
            result.DecryptedText = _aes.Decrypt(requestProtocolMessageModel.Payload, aesKey, requestProtocolMessageModel.IV);
            result.IsSucess = true;
        }
        catch (Exception ex)
        {
            Log.Error($"Cannot decrypt message #({requestProtocolMessageModel.Payload})", ex);
            return result;
        }
        
        return result;
    }
}