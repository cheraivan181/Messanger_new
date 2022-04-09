using Core.SessionServices.Domain;

namespace Core.SessionServices.Services.Interfaces;

public interface ISessionGetterService
{
    Task<SessionModel> GetSessionAsync(Guid userId, Guid sessionId);
}