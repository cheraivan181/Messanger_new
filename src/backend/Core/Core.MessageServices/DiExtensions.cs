using Core.MessageServices.Services.Implementations;
using Core.MessageServices.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.MessageServices;

public static class DiExtensions
{
    public static void AddMessageServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IMessageCacheService, MessageCacheService>();
        serviceCollection.AddSingleton<IMessageDispatcherService, MessageDispatcherService>();
        serviceCollection.AddSingleton<IMessageGetterService, MessageGetterService>();
        serviceCollection.AddSingleton<IMessageManagerService, MessageManagerService>();
        serviceCollection.AddSingleton<ISenderService, SenderService>();
        serviceCollection.AddSingleton<IMessageCacheInitializerService, MessageCacheInitializerService>();
    }
}