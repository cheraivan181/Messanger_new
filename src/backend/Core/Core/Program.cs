using Core;
using Core.Converters;
using Core.Hubs;
using Core.Identity;
using Core.Middlewhere;
using Core.Utils;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddHangfire(configuration => configuration
                 .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                 .UseSimpleAssemblyNameTypeSerializer()
                 .UseRecommendedSerializerSettings()
                 .UseSqlServerStorage(builder.Configuration.GetConnectionString("MsSql"), new SqlServerStorageOptions
                 {
                     CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                     SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                     QueuePollInterval = TimeSpan.Zero,
                     UseRecommendedIsolationLevel = true,
                     DisableGlobalLocks = true
                 }));


builder.Services.AddOptions();
builder.Services.AddLocalization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new IntPtrConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Messanger api",
        Description = "An ASP.NET Core Web API for realize messanger clients",
        License = new OpenApiLicense
        {
            Name = "MessangerLicense"
        }
    });
});

builder.Services.AddAntiforgery(options => { options.HeaderName = "x-xsrf-token"; });
builder.Services.Configure<CookiePolicyOptions>(options =>
{
});
builder.Services.AddDataProtection();
builder.Services.AddHttpClient();

builder.Services.AddAppInfrastructureServices(builder.Configuration);

builder.Services.AddSignalR(options =>
{
   
}).AddMessagePackProtocol(options =>
  {
        
  });

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

app.MapControllers().RequireCors("MessangerPolicy");
app.MapHub<MessangerHub>("/messangerhub").RequireCors("MessangerPolicy");

app.MapHangfireDashboard();

Log.Debug($"Application is running...");


app.Run();



void AddConfigureAuthenticationAndAuthorization(IServiceCollection collection, ConfigurationManager configuration)
{
    var signDecodingKey = new SignInSymmetricKey(configuration["TokenOptions:Key"]);
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
                      IssuerSigningKey = signDecodingKey.GetKey(),
                      ClockSkew = TimeSpan.Zero,
                      ValidateLifetime = true,
                      ValidateAudience = true,
                      ValidateIssuer = true,
                      ValidateIssuerSigningKey = true,
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

