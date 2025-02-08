namespace SagarImitation.Model.Settings
{
    public class SMTPSettings
    {
        public required string EmailEnableSsl { get; set; }
        public required string EmailHostName { get; set; }
        public required string EmailPassword { get; set; }
        public required string EmailPort { get; set; }
        public required string EmailUsername { get; set; }
        public required string FromEmail { get; set; }
        public required string EmailAppPassword { get; set; }
        public required string FromName { get; set; }
    }
}