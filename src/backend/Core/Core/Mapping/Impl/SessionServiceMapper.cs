using Core.ApiResponses.Session;
using Core.Mapping.Interfaces;

namespace Core.Mapping.Impl;

public class SessionServiceMapper : ISessionServiceMapper
{
    public CreateSessionResponse MapSessionResponse(SessionServices.Domain.CreateSessionResponse model)
    {
        var result = new CreateSessionResponse();
        result.SessionId = model.SessionId;
        result.ServerPublicKey = model.ServerPublicKey;
        result.IsNeedUpdateToken = model.IsNeedUpdateToken;

        return result;
    }
}