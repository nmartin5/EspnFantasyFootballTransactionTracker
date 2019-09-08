using EspnFantasyFootballTransactionTracker.Model;
using Microsoft.EntityFrameworkCore;

namespace EspnFantasyFootballTransactionTracker.Infrastructure.Persistence
{
    public class ActivityContext : DbContext
    {
        public DbSet<ActivityItem> ActivityItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite("Data Source=activity.db");
        }
    }
}