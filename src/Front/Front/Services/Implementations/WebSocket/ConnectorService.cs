using Blazored.LocalStorage;
using Front.Services.Interfaces.WebSocket;
using Microsoft.AspNetCore.SignalR.Client;

namespace Front.Services.Implementations.WebSocket
{
    public class ConnectorService : IConnectorService
    {
        private HubConnection _hubConnection;

        private ILocalStorageService _localStorageService;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public ConnectorService(IConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public async Task InitConnectionAsync()
        {
            var url = _configuration.GetValue<string>("ConnectorUrl");
            using var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _localStorageService = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();

            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{url}/messanger", options =>
                {
                    options.AccessTokenProvider = async () => 
                    {
                        var acessToken = await _localStorageService.GetItemAsStringAsync(Constants.AcessTokenName);
                        return acessToken;
                    };
                })
                .AddMessagePackProtocol()
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On("test", () => { });
            await _hubConnection.StartAsync();
        }
    }
}
    