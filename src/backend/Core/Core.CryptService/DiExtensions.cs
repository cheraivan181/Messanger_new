using Core.CryptService.Impl;
using Core.CryptService.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.CryptService
{
    public static class DiExtensions
    {
        public static void AddCryptService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IHashService, HashService>();
            serviceCollection.AddSingleton<IAesCypher, AesCypher>();
            serviceCollection.AddSingleton<IRsaCypher, RsaCypher>();
            serviceCollection.AddSingleton<IHmacService, HmacService>();
        }
    }
}
