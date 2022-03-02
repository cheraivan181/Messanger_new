using Core;
using Core.Hubs;
using Core.Middlewhere;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddOptions();
builder.Services.AddLocalization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {

    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAntiforgery(options => { options.HeaderName = "x-xsrf-token"; });
builder.Services.Configure<CookiePolicyOptions>(options =>
{
});
builder.Services.AddDataProtection();
builder.Services.AddHttpClient();

builder.Services.AddAppInfrastructureServices();

builder.Services.AddSignalR()
    .AddMessagePackProtocol();

AddConfigureLogging();
AddConfigureAuthenticationAndAuthorization(builder.Services, builder.Configuration);
AddCorsConfig(builder.Services, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                       ForwardedHeaders.XForwardedProto //nginx
});

app.UseCookiePolicy();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("MessangerPolicy");
app.UseMiddleWhares();

app.MapControllers();
app.MapHub<MessangerHub>("/messangerhub");

Log.Debug($"Application is running...");


app.Run();



void AddConfigureAuthenticationAndAuthorization(IServiceCollection collection, ConfigurationManager configuration)
{
    collection
              .AddAuthentication(options =>
              {
                  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
              })
              .AddJwtBearer(cfg =>
              {
                  cfg.RequireHttpsMetadata = false;
                  cfg.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidIssuer = configuration["Auth:Issuer"],
                      ValidAudience = configuration["Auth:Audience"],
                    // IssuerSigningKey = signingDecodingKey.GetKey(),
                      ClockSkew = TimeSpan.Zero,
                      ValidateLifetime = true,
                      ValidateAudience = false,
                      ValidateIssuer = true,
                      ValidateIssuerSigningKey = true
                  };

                  cfg.Events = new JwtBearerEvents
                  {
                      OnMessageReceived = context =>
                      {
                          var accessToken = context.Request.Query["access_token"];
                          var path = context.HttpContext.Request.Path;

                          if (path.StartsWithSegments("/messanger"))
                          {
                              context.Token = accessToken;
                          }

                          return Task.CompletedTask;
                      }
                  };
              });
    collection.AddAuthorization();
}
void AddConfigureLogging()
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .WriteTo.File(Path.Combine("c:/Logs", "logs", "log-.txt"),
            rollingInterval: RollingInterval.Day,
            restrictedToMinimumLevel: LogEventLevel.Debug)
        .WriteTo.Console(LogEventLevel.Debug)
        .CreateLogger();
}
void AddCorsConfig(IServiceCollection serviceCollection, ConfigurationManager configManager)
{
    var cors = configManager.GetValue<string>("Cors").Split(",");

    serviceCollection.AddCors(options => options.AddPolicy("MessangerPolicy",
                   policyOptions =>
                   {
                       policyOptions.WithOrigins(cors)
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials()
                                    .WithExposedHeaders();
                   }));
}

