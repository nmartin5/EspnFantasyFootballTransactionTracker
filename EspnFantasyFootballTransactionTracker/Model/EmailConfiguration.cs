namespace EspnFantasyFootballTransactionTracker.Model
{
    public class EmailConfiguration
    {
        public string SmtpDomain { get; set; }
        public int SmtpPort { get; set; }
        public string AuthorEmailAddress { get; set; }
        public string AuthorEmailPassword { get; set; }
        public string UserFullName { get; set; }
        public string RecipientList { get; set; }
    }
}
