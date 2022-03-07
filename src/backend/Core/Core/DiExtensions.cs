using Core.CryptService;
using Core.DbModels.Base.Di;
using Core.IdentityService;
using Core.Mapping;
using Core.Repositories;

namespace Core
{
    public static class DiExtensions
    {
        public static void AddAppInfrastructureServices(this IServiceCollection serviceCollection, ConfigurationManager configManager)
        {
            serviceCollection.AddDbServices();
            serviceCollection.AddRepositories();
            serviceCollection.AddIdentityServices(configManager);
            serviceCollection.AddMappers();
            serviceCollection.AddCryptService();
        } 
    }
}
