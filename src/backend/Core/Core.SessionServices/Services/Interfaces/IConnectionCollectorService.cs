namespace Core.SessionServices.Services.Interfaces;

public interface IConnectionCollectorService
{
    Task AddConnectionAsync(long userId, long sessionId, string connectionId);
    Task RemoveConnectionAsync(long userId, string connectionId);
}