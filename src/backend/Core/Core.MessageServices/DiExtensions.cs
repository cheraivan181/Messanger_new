using Core.MessageServices.Services.Implementations;
using Core.MessageServices.Services.Implementations.Handlers.Implementations;
using Core.MessageServices.Services.Implementations.Handlers.Interfaces;
using Core.MessageServices.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.MessageServices;

public static class DiExtensions
{
    public static void AddMessageServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IMessageCacheService, MessageCacheService>();
        serviceCollection.AddScoped<IMessageDispatcherService, MessageDispatcherService>();
        serviceCollection.AddScoped<IMessageGetterService, MessageGetterService>();
        serviceCollection.AddScoped<IMessageManagerService, MessageManagerService>();
        serviceCollection.AddScoped<ISenderService, SenderService>();
        serviceCollection.AddScoped<IMessageCacheInitializerService, MessageCacheInitializerService>();
        serviceCollection.AddScoped<IMessageMapper, MessageMapper>();
        #region handlers
        
        serviceCollection.AddScoped<IDirectMessageHandler, DirectMessageHandler>();
        serviceCollection.AddScoped<IUpdateMessageHandler, UpdateMessageHandler>();
        serviceCollection.AddScoped<IGetDialogMessageHandler, GetDialogMessagesHandler>();
        serviceCollection.AddScoped<IErrorMessageHandler, ErrorMessageHandler>();

        #endregion
    }
}