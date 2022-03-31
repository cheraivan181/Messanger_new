using ConnectionHandler;
using Serilog;

public class Program
{
    public static async Task Main(string[] args)
    {
        var hostBuilder = BuildHost(args);
        var host = hostBuilder.Build();
        Log.Debug("Application is running...");
        
        await host.RunAsync();
    }

    private static IHostBuilder BuildHost(string[] args) =>
        Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseSerilog();

                webBuilder.ConfigureAppConfiguration((builderContext, configs) =>
                {

                });
            });
}