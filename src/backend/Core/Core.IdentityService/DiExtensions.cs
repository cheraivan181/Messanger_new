using Core.IdentityService.Domain.Options;
using Core.IdentityService.Interfaces;
using Core.IdentityService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.IdentityService
{
    public static class DiExtensions
    {
        public static void AddIdentityServices(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.AddScoped<IIdentityService, IdentityService>();
            serviceCollection.AddScoped<IJwtService, JwtService>();

            serviceCollection.Configure<TokenLifeTimeOptions>(configuration.GetSection("TokenOptions:TokenLifeTimeOptions"));
            serviceCollection.Configure<TokenOptions>(configuration.GetSection("TokenOptions"));
            serviceCollection.Configure<AuthOptions>(configuration.GetSection("Auth"));
        }
    }
}
