using System.Reflection;
using Core.MessageServices.Services.Implementations.Handlers.Interfaces;
using Core.MessageServices.Services.Implementations.Senders.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.HandlerActions;

public static class DiExtensions
{
    public static void AddHandlerActionsServices(this IServiceCollection serviceCollection)
    {
        var handlerType = typeof(IHandler);
        var assemblyTypes = handlerType.Assembly.GetTypes();

        var allHandlerClasses = assemblyTypes
            .Where(x => x.IsClass && x.IsPublic)
            .Where(x => !x.IsAbstract)
            .Where(x => !x.IsInterface)
            .Where(x => handlerType.IsAssignableFrom(x));
        
        foreach (var handlerClass in allHandlerClasses)
        {
            serviceCollection.AddScoped(handlerType, handlerClass);
        }

        var senderType = typeof(ISender);
        var allSenderClasses = assemblyTypes
            .Where(x => x.IsClass && x.IsPublic)
            .Where(x => !x.IsAbstract)
            .Where(x => !x.IsInterface)
            .Where(x => senderType.IsAssignableFrom(x));

        foreach (var senderClass in allSenderClasses)
        {
            serviceCollection.AddScoped(senderType, senderClass);
        }
    }
}