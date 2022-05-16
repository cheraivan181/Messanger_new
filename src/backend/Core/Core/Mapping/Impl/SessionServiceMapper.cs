using Core.ApiResponses.Session;
using Core.Mapping.Interfaces;

namespace Core.Mapping.Impl;

public class SessionServiceMapper : ISessionServiceMapper
{
    public CreateSessionResponse Map(SessionServices.Domain.CreateSessionResponse model)
    {
        var result = new CreateSessionResponse();
        result.SessionId = model.SessionId;
        result.ServerPublicKey = model.ServerPublicKey;
        result.IsNeedUpdateToken = model.IsNeedUpdateToken;
        result.HmacKey = model.HmacKey;
        result.Aes = model.AesKey;
    
        return result;
    }
}