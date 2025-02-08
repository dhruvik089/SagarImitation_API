namespace SagarImitation.Model.Settings
{
    public class EmailTemplates
    {
        public required string ExceptionReport {  get; set; }
        public required string UserForgotPassword {  get; set; }
        public required string UserPassword {  get; set; }
        public required string UserForgotPasswordWithLink { get; set; }
    }
}