namespace SagarImitation.Model.Settings
{
    public class AppSettings
    {
        public required string JWT_Secret { get; set; }
        public required int JWT_Validity_Mins { get; set; }
        public required string ExceptionEmailSend { get; set; }
        public required int ForgotPasswordAttemptValidityHours { get; set; }
        public required int PasswordLinkValidityMins { get; set; }
        public required bool Caching {  get; set; }
    }
}