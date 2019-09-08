using System;

namespace EspnFantasyFootballTransactionTracker.Model
{
    public class ActivityItem
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Detail { get; set; }
        public DateTime Date { get; set; }

        public ActivityItem(string type, string detail, DateTime activityDate)
        {
            Id = Guid.NewGuid();

            Type = type;
            Detail = detail;

            Date = activityDate.ToUniversalTime();
        }

        private ActivityItem() { }
    }
}
