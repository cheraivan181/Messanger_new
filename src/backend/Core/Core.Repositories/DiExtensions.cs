using Core.Repositories.Impl;
using Core.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Repositories
{
    public static class DiExtensions
    {
        public static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IRoleRepository, RoleRepository>();
            serviceCollection.AddScoped<IUserRoleRepository, UserRoleRepository>();
            serviceCollection.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            serviceCollection.AddScoped<ISessionRepository, SessionRepository>();
            serviceCollection.AddScoped<IConnectionRepository, ConnectionRepository>();
            serviceCollection.AddScoped<IDialogRepository, DialogRepository>();
            serviceCollection.AddScoped<IDialogRequestRepository, DialogRequestRepository>();
            serviceCollection.AddScoped<IDialogSecretRepository, DialogSecretRepository>();
            serviceCollection.AddScoped<IMessageRepository, MessageRepository>();
            serviceCollection.AddScoped<IUserCypherKeyRepository, UserCypherKeyRepository>();
        }
    }
}
