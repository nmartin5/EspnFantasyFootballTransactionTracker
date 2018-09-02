using System.Threading.Tasks;

namespace EspnFantasyFootballTransactionTracker
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string subject, string message);
    }
}
