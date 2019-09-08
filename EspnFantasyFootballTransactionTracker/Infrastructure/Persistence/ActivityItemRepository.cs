using System;
using System.Linq;
using EspnFantasyFootballTransactionTracker.Model;

namespace EspnFantasyFootballTransactionTracker.Infrastructure.Persistence
{
    public class ActivityItemRepository : IActivityItemRepository
    {
        public ActivityContext DbContext { private get; set; }

        public void Add(ActivityItem item)
        {
            if (DbContext == null)
                throw new ArgumentNullException($"{nameof(DbContext)} cannot be null!");

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            throw new NotImplementedException();
        }

        public ActivityItem Get(DateTime date, string detail)
        {
            if (DbContext == null)
                throw new ArgumentNullException($"{nameof(DbContext)} cannot be null!");

            if (date == null)
                throw new ArgumentNullException(nameof(date));

            if (string.IsNullOrEmpty(detail))
                throw new ArgumentNullException(nameof(detail));

            if (DbContext.ActivityItems.Any(i => i.Date == date.ToUniversalTime() && i.Detail == detail))
            {
                return DbContext.ActivityItems.Single(i => i.Date == date.ToUniversalTime() && i.Detail == detail);
            }

            return null;
        }
    }
}
