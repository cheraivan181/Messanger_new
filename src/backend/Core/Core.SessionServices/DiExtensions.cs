using Core.SessionServices.Services.Implementations;
using Core.SessionServices.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.SessionServices;

public static class DiExtensions
{
    public static void AddSessionServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ISessionService, SessionService>();
        serviceCollection.AddScoped<ISessionGetterService, SessionGetterService>();
        serviceCollection.AddScoped<ISessionCacheService, SessionCacheService>();
        serviceCollection.AddScoped<IConnectionCollectorCacheService, ConnectionCollectorCacheService>();
        serviceCollection.AddScoped<IConnectionCollectorService, ConnectionCollectorService>();
        serviceCollection.AddScoped<ISessionCacheInitializerService, SessionCacheInitializerService>();
    }
}