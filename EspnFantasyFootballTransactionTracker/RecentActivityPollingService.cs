using System;
using System.Threading;
using System.Threading.Tasks;
using EspnFantasyFootballTransactionTracker.Infrastructure;
using EspnFantasyFootballTransactionTracker.Infrastructure.Persistence;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EspnFantasyFootballTransactionTracker
{
    public class RecentActivityPollingService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly ILeagueActivityScraper _scraper;
        private readonly IActivityContextFactory _contextFactory;
        private readonly INotificationService _notificationService;
        private readonly IActivityItemRepository _activityItemRepo;
        private Timer _timer;
        public RecentActivityPollingService(
            ILogger logger,
            ILeagueActivityScraper scraper,
            IActivityContextFactory contextFactory,
            INotificationService notificationService,
            IActivityItemRepository activityItemRepo)
        {
            _logger = logger;
            _scraper = scraper;
            _contextFactory = contextFactory;
            _notificationService = notificationService;
            _activityItemRepo = activityItemRepo;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(RecentActivityPollingService)} is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation($"{nameof(RecentActivityPollingService)} is working.");

            var items = _scraper.GetActivityItems();

            using var context = _contextFactory.Build();
            _activityItemRepo.DbContext = context;
            foreach (var item in items)
            {
                if (_activityItemRepo.Get(item.Date, item.Detail) != null)
                {
                    _activityItemRepo.Add(item);
                    _notificationService.NotifyAsync($"New Activity. {item.Type}. {item.Detail}");
                }
            }
            context.SaveChanges();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(RecentActivityPollingService)} is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}