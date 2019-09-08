using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EspnFantasyFootballTransactionTracker.Infrastructure;
using EspnFantasyFootballTransactionTracker.Infrastructure.Persistence;

namespace EspnFantasyFootballTransactionTracker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
            .ConfigureLogging((hostContext, config) =>
            {
                config.AddConsole();
                config.AddDebug();
            })
            .ConfigureHostConfiguration(config => config.AddEnvironmentVariables())
            .ConfigureAppConfiguration((hostContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true);
                config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                config.AddCommandLine(args);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddLogging();
                services.AddHostedService<RecentActivityPollingService>();
                services.AddScoped<ILeagueActivityScraper, LeagueActivityScraper>();
                services.AddScoped<INotificationService, TwilioSMSNotificationService>();
                services.AddScoped<IActivityContextFactory, ActivityContextFactory>();
                services.AddScoped<IActivityItemRepository, ActivityItemRepository>();
            })
                .UseConsoleLifetime()
                .Build();

            using (host)
            {
                await host.StartAsync();

                await host.WaitForShutdownAsync();
            }
        }
    }
}
