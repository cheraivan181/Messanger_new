using Core.Mapping.Impl;
using Core.Mapping.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Mapping
{
    public static class DiExtensions
    {
        public static void AddMappers(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IIdentityServiceMapper, IdentityServiceMapper>();
        }
    }
}
