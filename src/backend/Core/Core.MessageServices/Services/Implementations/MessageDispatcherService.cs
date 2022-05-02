using System.Collections.Concurrent;
using System.Linq.Expressions;
using Core.CryptProtocol.Domain;
using Core.CryptProtocol.Domain.Base;
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
    private readonly IErrorMessageHandler _errorMessageHandler;

    private ConcurrentDictionary<RequestType, Func<string, string, string, ParsedResult<object>>>
        _convertersFuncs;

    private ConcurrentDictionary<RequestType, Type> _messageTypes;
    private readonly Type _parseRequestType;

    public MessageDispatcherService(IDirectMessageHandler directMessageHandler,
        ISessionGetterService sessionGetterService,
        IErrorMessageHandler errorMessageHandler)
    {
        _directMessageHandler = directMessageHandler;
        _sessionGetterService = sessionGetterService;
        _errorMessageHandler = errorMessageHandler;
        _convertersFuncs =
            new ConcurrentDictionary<RequestType, Func<string, string, string, ParsedResult<object>>>();
        _convertersFuncs = new ConcurrentDictionary<RequestType, Func<string, string, string, ParsedResult<object>>>();
    }
    
    public async Task DispatchMessage(DispatchMessageRequest request)
    {
        var session = await _sessionGetterService.GetSessionAsync(request.UserId, request.SessionId);
        var parsedRequestFunc = GetParsedRequestFunc(request.RequestType);
        
        var parsedResult = parsedRequestFunc(session.HmacKey, session.AesKey, request.Message);

        switch (request.RequestType)
        {
            case RequestType.DirectMessage:
                await _directMessageHandler.HandleAsync(parsedResult.RequestModel as SendDirectMessageRequest);
                break;
        }

        if (!parsedResult.IsSucess)
        {
            await _errorMessageHandler.HandleAsync(request.UserId, Guid.Empty, request.ConnectionId,
                "Cannot parse model");
        }
    }
    
    private Func<string, string, string, ParsedResult<object>> GetParsedRequestFunc(RequestType requestType)
    {
        if (_convertersFuncs.TryGetValue(requestType, out var res))
            return res;
        
        var requestParserType = typeof(RequestParser);
        var type = GetTypeByMessageType(requestType);   
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
        _convertersFuncs.TryAdd(requestType, res);
        
        return res;
    }

    private Type GetTypeByMessageType(RequestType requestType)
    {
        if (_messageTypes.TryGetValue(requestType, out var result))
            return result;
        
        switch (requestType)
        {
            case RequestType.DirectMessage:
            {
                result = typeof(SendDirectMessageRequest);
                _messageTypes.TryAdd(requestType, result);
                break;
            }
            default:
                throw new Exception("Cannot get message type");
        }

        return result;
    }
}