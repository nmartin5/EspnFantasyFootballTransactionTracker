namespace EspnFantasyFootballTransactionTracker.Infrastructure.Persistence
{
    public class ActivityContextFactory : IActivityContextFactory
    {
        public ActivityContext Build()
        {
            return new ActivityContext();
        }
    }
}