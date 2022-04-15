using Core.ApiResponses.Session;

namespace Core.Mapping.Interfaces;

public interface ISessionServiceMapper
{
    CreateSessionResponse Map(SessionServices.Domain.CreateSessionResponse model);
}