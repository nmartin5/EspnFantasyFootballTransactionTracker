using System.Threading.Tasks;

namespace EspnFantasyFootballTransactionTracker
{
    public interface INotificationService
    {
        Task NotifyAsync(string message);
    }
}
