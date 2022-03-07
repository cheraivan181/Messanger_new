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
            ConfigurationManager configurationManager)
        {
            serviceCollection.AddScoped<IIdentityService, IdentityService>();
            serviceCollection.AddScoped<IJwtService, JwtService>();

            serviceCollection.Configure<TokenLifeTimeOptions>(configurationManager.GetSection("TokenOptions:TokenLifeTimeOptions"));
            serviceCollection.Configure<TokenOptions>(configurationManager.GetSection("TokenOptions"));
            serviceCollection.Configure<AuthOptions>(configurationManager.GetSection("Auth"));
        }
    }
}
