using EspnFantasyFootballTransactionTracker.Model;
using System.Collections.Generic;

namespace EspnFantasyFootballTransactionTracker.Infrastructure
{
    public interface ILeagueActivityScraper
    {
        List<ActivityItem> GetActivityItems();
    }
}
