using Core.DialogServices.Implementations;
using Core.DialogServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.DialogServices;

public static class DiExtensions
{
    public static void AddDialogServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IDialogRequestService, DialogRequestService>();
        serviceCollection.AddScoped<IDialogService, DialogService>();
    }
}