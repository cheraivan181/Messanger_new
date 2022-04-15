using Front.Domain.Session;

namespace Front.Services.Interfaces.Sessions
{
    public interface ISessionGetterService
    {
        Task<SessionModel> GetSessionAsync();
    }
}
