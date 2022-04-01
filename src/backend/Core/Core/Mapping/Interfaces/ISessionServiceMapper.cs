using Core.ApiResponses.Session;

namespace Core.Mapping.Interfaces;

public interface ISessionServiceMapper
{
    CreateSessionResponse MapSessionResponse(SessionServices.Domain.CreateSessionResponse model);
}