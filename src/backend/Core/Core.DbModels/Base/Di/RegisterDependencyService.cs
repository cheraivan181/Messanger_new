using Core.DbModels.Base.Interface;
using Microsoft.Extensions.DependencyInjection;


namespace Core.DbModels.Base.Di
{
    public static class RegisterDependencyService
    {
        public static void AddDbServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IConnectionFactory, ConnectionFactory>();
            serviceCollection.AddSingleton<ITransactionProvider, TransactionProvider>();
        }
    }
}
