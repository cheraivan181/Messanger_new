using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Front;
using Front.Clients.Implementations;
using Front.Clients.Interfaces;
using Front.Services.Implementations.Alive;
using Front.Services.Implementations.Auth;
using Front.Services.Implementations.Crypt;
using Front.Services.Implementations.Dialogs;
using Front.Services.Implementations.Sessions;
using Front.Services.Implementations.WebSocket;
using Front.Services.Interfaces.Alive;
using Front.Services.Interfaces.Auth;
using Front.Services.Interfaces.Crypt;
using Front.Services.Interfaces.Dialogs;
using Front.Services.Interfaces.Sessions;
using Front.Services.Interfaces.WebSocket;
using Front.Servives.Implementations;
using Front.Servives.Implementations.Auth;
using Front.Servives.Interfaces.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Configuration.AddInMemoryCollection(Configuration.Configurations);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseAdress = builder.Configuration.GetValue<string>("CoreUrl");
builder.Services.AddHttpClient("CoreClient", httpClient =>
{
    httpClient.BaseAddress = new Uri(baseAdress);
});

#region httpClients

builder.Services.AddScoped<IRestClient, RestClient>();
builder.Services.AddScoped<IAliveClient, AliveClient>();
builder.Services.AddScoped<IAccountClient, AccountClient>();
builder.Services.AddScoped<ISessionClient, SessionClient>();
builder.Services.AddScoped<ICryptClient, CryptClient>();
builder.Services.AddScoped<ISearchClient, SearchClient>();
builder.Services.AddScoped<IDialogClient, DialogClient>();

#endregion


#region alives

builder.Services.AddScoped<IAliveService, AliveService>();

#endregion

#region authServices

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthOptionsService, AuthOptionsService>();
builder.Services.AddScoped<IAuthService, AuthService>();

#endregion

#region cryptServices

builder.Services.AddScoped<IAesCryptService, AesService>();
builder.Services.AddScoped<IRsaService, RsaService>();

#endregion

#region sessionServices

builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ISessionGetterService, SessionGetterService>();

#endregion

#region connectorServices

builder.Services.AddSingleton<IConnectorService, ConnectorService>();

#endregion

#region dialogServices

builder.Services.AddScoped<IDialogManagerService, DialogService>();

#endregion

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
