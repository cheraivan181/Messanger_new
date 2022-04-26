using System.Collections.Concurrent;
using System.Linq.Expressions;
using Core.CryptProtocol.Domain;
using Core.CryptProtocol.Services.Implementations;
using Core.CryptProtocol.Services.Interfaces;
using Core.MessageServices.Domain;
using Core.MessageServices.Services.Implementations.Handlers.Implementations;
using Core.MessageServices.Services.Implementations.Handlers.Interfaces;
using Core.MessageServices.Services.Interfaces;
using Core.SessionServices.Services.Interfaces;

namespace Core.MessageServices.Services.Implementations;

public class MessageDispatcherService : IMessageDispatcherService
{
    private readonly IDirectMessageHandler _directMessageHandler;
    private readonly ISessionGetterService _sessionGetterService;

    private ConcurrentDictionary<MessageType, Func<string, string, string, ParsedResult<object>>>
        _convertersFuncs;

    private ConcurrentDictionary<MessageType, Type> _messageTypes;
    private readonly Type _parseRequestType;

    public MessageDispatcherService(IDirectMessageHandler directMessageHandler,
        ISessionGetterService sessionGetterService)
    {
        _directMessageHandler = directMessageHandler;
        _sessionGetterService = sessionGetterService;
        _convertersFuncs =
            new ConcurrentDictionary<MessageType, Func<string, string, string, ParsedResult<object>>>();
        _convertersFuncs = new ConcurrentDictionary<MessageType, Func<string, string, string, ParsedResult<object>>>();
    }
    
    public async Task DispatchMessage(Guid userId, 
        Guid sessionId,
        string message,
        MessageType messageType)
    {
        var session = await _sessionGetterService.GetSessionAsync(userId, sessionId);
        var parsedRequestFunc = GetParsedRequestFunc(messageType);
        
        var parsedResult = parsedRequestFunc(session.HmacKey, session.AesKey, message);

        switch (messageType)
        {
            case MessageType.DirectMessage:
                parsedResult.RequestModel = parsedResult.RequestModel as SendDirectMessageRequest;
                break;
        }

        if (!parsedResult.IsSucess)
        {
            //use error handler
        }
    }
    
    private Func<string, string, string, ParsedResult<object>> GetParsedRequestFunc(MessageType messageType)
    {
        if (_convertersFuncs.TryGetValue(messageType, out var res))
            return res;
        
        var requestParserType = typeof(RequestParser);
        var type = GetTypeByMessageType(messageType);   
        var parseRequestMethodInfo = requestParserType.GetMethod(nameof(RequestParser.ParseRequest))
            .MakeGenericMethod(type);

        var hmacKeyParameter = Expression.Parameter(typeof(string), "hmacKey");
        var aesKeyParameter = Expression.Parameter(typeof(string), "aesKey");
        var message = Expression.Parameter(typeof(string), "message");

        var call = Expression.Call(parseRequestMethodInfo,
            hmacKeyParameter,
            aesKeyParameter,
            message);

        var lambda = Expression.Lambda<Func<string, string, string, ParsedResult<object>>>
            (call, hmacKeyParameter, aesKeyParameter, message);

        res = lambda.Compile();
        _convertersFuncs.TryAdd(messageType, res);
        
        return res;
    }

    private Type GetTypeByMessageType(MessageType messageType)
    {
        if (_messageTypes.TryGetValue(messageType, out var result))
            return result;
        
        switch (messageType)
        {
            case MessageType.DirectMessage:
            {
                result = typeof(SendDirectMessageRequest);
                _messageTypes.TryAdd(messageType, result);
                break;
            }
            default:
                throw new Exception("Cannot get message type");
        }

        return result;
    }
}