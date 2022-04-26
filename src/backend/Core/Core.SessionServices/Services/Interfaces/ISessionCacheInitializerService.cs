namespace Core.SessionServices.Services.Interfaces;

public interface ISessionCacheInitializerService
{
    Task SetSessionInCacheAsync(Guid sessionId,
        Guid userId);
}