using EspnFantasyFootballTransactionTracker.Model;
using System.Collections.Generic;

namespace EspnFantasyFootballTransactionTracker.Scraping
{
    public interface ILeagueActivityScraper
    {
        ICollection<ActivityItem> GetNewActivityItems();
    }
}
