using System;
using System.IO;
using Core.Converters;
using Core.Identity;
using Core.Middlewhere;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Core.DbModels.Base;
using Core.HostingServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;

namespace Core
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public IWebHostEnvironment Environment { get; set; }

        public Startup(IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddLocalization();
            services.AddHttpContextAccessor();
            services.AddControllers()
                 .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.Converters.Add(new IntPtrConverter());
                     options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                 });

            services.AddEndpointsApiExplorer();

            services.AddAntiforgery(options => { options.HeaderName = "x-xsrf-token"; });
            services.Configure<CookiePolicyOptions>(options =>
            {
            });

            services.AddDataProtection();
            services.AddHttpClient();

            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MsSql"));
            });

            services.AddAppInfrastructureServices(Configuration);

            services.AddHostedService<ReceiveMessageHostingService>();
            
            services.AddSignalR(options =>
            {

            }).AddMessagePackProtocol(options =>
            {

            });

            AddCorsConfig(services, Configuration);
            AddConfigureAuthenticationAndAuthorization(services, Configuration);
            AddConfigureLogging();
            AddHangfireServices(services);
            AddSwaggerServices(services);
        }

        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            MetricsCollectorStart();
            
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                       ForwardedHeaders.XForwardedProto //nginx
            });

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("MessangerPolicy");
            app.UseMiddleWhares();
            app.UseMetricServer();
            app.UseHttpMetrics();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
                endpoints.MapControllers();
//                endpoints.MapHub<MessangerHub>("/messangerhub").RequireCors("MessangerPolicy");
                endpoints.MapHangfireDashboard();
            });

        }

        private void AddHangfireServices(IServiceCollection services)
        {
            services.AddHangfire(configuration => configuration
                 .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                 .UseSimpleAssemblyNameTypeSerializer()
                 .UseRecommendedSerializerSettings()
                 .UseSqlServerStorage(Configuration.GetConnectionString("MsSql"), new SqlServerStorageOptions
                 {
                     CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                     SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                     QueuePollInterval = TimeSpan.Zero,
                     UseRecommendedIsolationLevel = true,
                     DisableGlobalLocks = true
                 }));
        }

        private void AddSwaggerServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(options =>
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
        }
        private void AddConfigureAuthenticationAndAuthorization(IServiceCollection collection, IConfiguration configuration)
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
        private void AddConfigureLogging()
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
        private void AddCorsConfig(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var cors = configuration.GetValue<string>("Cors").Split(",");

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
        
        
        private void MetricsCollectorStart()
        {
            var metricServer = new KestrelMetricServer(port: 1234);
            metricServer.Start();
        }
    }
}
