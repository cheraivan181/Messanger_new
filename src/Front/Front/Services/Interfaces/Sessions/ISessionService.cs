using Front.Domain.Session;

namespace Front.Services.Interfaces.Sessions
{
    public interface ISessionService
    {
        Task<bool> CreateSessionService();
        Task<bool> IsNeedCreateSessionAsync();
    }
}
