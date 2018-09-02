using EspnFantasyFootballTransactionTracker.Model;
using EspnFantasyFootballTransactionTracker.Scraping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EspnFantasyFootballTransactionTracker
{
    class Process
    {
        private readonly ILeagueActivityScraper _leagueActivityScraper;
        private readonly IEmailSender _emailSender;

        public Process(ILeagueActivityScraper leagueActivityScraper, IEmailSender emailSender)
        {
            _leagueActivityScraper = leagueActivityScraper;
            _emailSender = emailSender;
        }

        public async Task StartAsync()
        {
            HashSet<ActivityItem> activityItemCache = new HashSet<ActivityItem>();

            while (true)
            {
                var items = _leagueActivityScraper.GetNewActivityItems();
                var newItems = new List<ActivityItem>();

                foreach (var item in items)
                {
                    if (!activityItemCache.Contains(item))
                    {
                        if (activityItemCache.Count == 50)
                        {
                            ActivityItem oldestItem = activityItemCache.OrderBy(x => x.DateTime).First();
                            activityItemCache.Remove(oldestItem);
                        }
                        activityItemCache.Add(item);
                        newItems.Add(item);
                    }
                }
                if (newItems.Any())
                {
                    List<string> newItemList = newItems.Select(x => $"{x.DateTime.ToShortDateString()}: {x.Description}").ToList();
                    string newItemListstr = string.Join(".\n", newItemList);

                    string body = $@"The following activity items occurred since your last update.

{newItemListstr}

EOM";

                    await _emailSender.SendEmailAsync("New League Activity", body);
                }

                await Task.Delay(new TimeSpan(0, 5, 0));
            }
        }
    }
}
