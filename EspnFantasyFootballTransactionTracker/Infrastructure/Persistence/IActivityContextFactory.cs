namespace EspnFantasyFootballTransactionTracker.Infrastructure.Persistence
{
    public interface IActivityContextFactory
    {
        ActivityContext Build();
    }
}