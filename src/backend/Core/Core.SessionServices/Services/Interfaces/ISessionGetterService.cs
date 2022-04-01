using Core.SessionServices.Domain;

namespace Core.SessionServices.Services.Interfaces;

public interface ISessionGetterService
{
    Task<SessionModel> GetSessionAsync(long userId, long sessionId);
}