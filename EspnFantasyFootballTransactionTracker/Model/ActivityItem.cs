using System;
using System.Collections.Generic;

namespace EspnFantasyFootballTransactionTracker.Model
{
    public class ActivityItem : IEquatable<ActivityItem>
    {
        public DateTime DateTime { get; private set; }
        public string Description { get; private set; }

        public ActivityItem(DateTime activityTime, string description)
        {
            DateTime = activityTime;
            Description = description;
        }

        public bool Equals(ActivityItem item)
        {
            return DateTime == item.DateTime && Description == item.Description;
        }

        public override int GetHashCode()
        {
            var hashCode = 1715533081;
            hashCode = hashCode * -1521134295 + DateTime.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            return hashCode;
        }
    }
}
