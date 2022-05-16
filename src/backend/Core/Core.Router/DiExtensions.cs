using Core.MessageServices.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.MessageServices.Services.Implementations;

public static class DiExtensions
{
    public static void AddRouterServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IMessageRouterService, MessageRouterService>();
    }
}