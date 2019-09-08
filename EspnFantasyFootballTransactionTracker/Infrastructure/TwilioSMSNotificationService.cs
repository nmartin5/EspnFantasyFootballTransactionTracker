using System.Threading.Tasks;

namespace EspnFantasyFootballTransactionTracker
{
    public class TwilioSMSNotificationService : INotificationService
    {
        Task INotificationService.NotifyAsync(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}
