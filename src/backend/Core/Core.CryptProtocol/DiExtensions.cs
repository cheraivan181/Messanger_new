using Core.CryptProtocol.Services.Implementations;
using Core.CryptProtocol.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.CryptProtocol;

public static class DiExtensions
{
    public static void AddProtocolServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IRequestParser, RequestParser>();
        serviceCollection.AddSingleton<IResponseBuilder, ResponseBuilder>();
    }
}