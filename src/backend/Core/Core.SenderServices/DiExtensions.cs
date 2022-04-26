using Core.MessageServices.Services.Implementations;
using Core.MessageServices.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.SenderServices;

public static class DiExtensions
{
    public static void AddSenderServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ISenderService, SenderService>();
        serviceCollection.AddSingleton<IPrepereMessagesToSendService, PrepereMessagesToSendService>();
        serviceCollection.AddSingleton<IMessageOffsetService, MessageOffsetService>();
    }
}