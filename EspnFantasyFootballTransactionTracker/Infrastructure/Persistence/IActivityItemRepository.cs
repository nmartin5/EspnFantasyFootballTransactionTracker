using System;
using EspnFantasyFootballTransactionTracker.Model;

namespace EspnFantasyFootballTransactionTracker.Infrastructure.Persistence
{
    public interface IActivityItemRepository
    {
        ActivityContext DbContext { set; }
        ActivityItem Get(DateTime date, string detail);
        void Add(ActivityItem item);
    }
}
