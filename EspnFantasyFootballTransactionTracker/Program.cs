using EspnFantasyFootballTransactionTracker.Model;
using EspnFantasyFootballTransactionTracker.Scraping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace EspnFantasyFootballTransactionTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();

            services.Configure<EmailConfiguration>(configuration.GetSection(nameof(EmailConfiguration)));
            services.Configure<LeagueConfiguration>(configuration.GetSection(nameof(LeagueConfiguration)));

            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<ILeagueActivityScraper, LeagueActivityScraper>();
            services.AddSingleton<Process>();

            ServiceProvider provider = services.BuildServiceProvider();

            var process = provider.GetService<Process>();

            try
            {
                process.StartAsync().Wait();
            }
            catch (Exception ex)
            {
                var emailService = provider.GetService<IEmailSender>();

                emailService.SendEmailAsync("Raspberry Pi Error!", $@"
An exception has occured.

Ex: {ex.ToString()}

Message: {ex.Message}

StackTrace: {ex.StackTrace}");
            }
        }
    }
}
