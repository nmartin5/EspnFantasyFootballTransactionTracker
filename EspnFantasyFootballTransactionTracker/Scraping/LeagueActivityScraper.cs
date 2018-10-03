using EspnFantasyFootballTransactionTracker.Model;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EspnFantasyFootballTransactionTracker.Scraping
{
    class LeagueActivityScraper : ILeagueActivityScraper
    {
        private readonly LeagueConfiguration _leagueConfig;

        public LeagueActivityScraper(IOptions<LeagueConfiguration> emailConfiguration)
        {
            _leagueConfig = emailConfiguration.Value;
        }

        public ICollection<ActivityItem> GetNewActivityItems()
        {
            return GetNewActivityItemsAsync().Result;
        }

        private async Task<ICollection<ActivityItem>> GetNewActivityItemsAsync()
        {
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"http://games.espn.com/ffl/leagueoffice?leagueId={_leagueConfig.LeagueId}&seasonId={_leagueConfig.SeasonId}");
            var pageContents = await response.Content.ReadAsStringAsync();

            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);

            var activityItems = pageDocument.DocumentNode.SelectNodes("//*[@id='lo-recent-activity-list']/li");

            var queriedActivityItems = activityItems.Where(t => t.HasClass("lo-recent-activity-item") && !t.HasClass("bottom-item"));

            int i = 0;

            List<ActivityItem> items = new List<ActivityItem>();
            foreach (var activityItem in queriedActivityItems)
            {
                activityItem.InnerHtml = activityItem.InnerHtml.Replace("<br>", "\n");
                string date = activityItem.SelectNodes("//li[@class='recent-activity-when']//span[@class='recent-activity-date']")[i].InnerText;

                string time = activityItem.SelectNodes("//li[@class='recent-activity-when']//span[@class='recent-activity-time']")[i].InnerText;

                var description = activityItem.SelectNodes("//li[@class='recent-activity-description']")[i].InnerText;
                var descriptionWithoutAsterisk = description.Replace('*', '');

                var dateTime = DateTime.ParseExact($"{date} {time}", "MMM d h:mm tt", new CultureInfo("en-US"));

                ActivityItem itemToAdd = new ActivityItem(dateTime, descriptionWithoutAsterisk);

                items.Add(itemToAdd);
                i++;
            }

            return items;
        }
    }
}
