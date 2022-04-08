using Core.SearchServices.Interfaces;
using Core.SearchServices.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.SearchServices;

public static class DiExtensions
{
    public static void AddSearchUserServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserSearchService, UserSearchService>();
    }
}