namespace SagarImitation.Common.Helpers
{
    public class ErrorMessages
    {
        //Login
        public const string InvalidEmailId = "Invalid email address.";
        public const string InvalidCredential = "Invalid Email ID or Password.";
        public const string LoginSuccess = "Login successful.";
        public const string SomethingWentWrong = "Something went wrong!! Please try again later.";
        public const string EmailIsRequired = "Email ID is required.";
        public const string EnterValidEmail = "Please a enter valid email.";
        public const string InvalidId = "ID must be greater then zero.";

        //ForgetPassowrd
        public const string ForgetPasswordSuccess = "Verification code send successfully. Please check your inbox.";
        public const string ForgetPasswordError = "An error occured while sending email! Please try again.";
        public const string UserError = "You're not registered with us.";
        public const string ForgotPasswordSuccessWithLink = "Reset password link sent successfully. Please check your inbox";

        //ValidateResetPassword
        public const string ValidateResetPasswordSuccess = "User is valid.";
        public const string URLExpired = "This URL is expired... Please try again!";

        //VerifyOPT
        public const string Verifycode = "Verification code verified successfully.";
        public const string Wrongcode = "Verification code does not match.";

        //ResetPassword
        public const string ResetPasswordSuccess = "Password changed successfully.";
        public const string ResetPasswordFail = "Invalid token please try again.";
        public const string ConfirmPassword = "New password and confirmation password does not match.";
        public const string PasswordValidationConfirm = "Confirm password is required.";
        public const string PasswordValidation = "Create password is required.";
        public const string StrongPassword = "Password must be at least 6 characters long, Contain at least one number and have a mixture of uppercase and lowercase letters.";
        public const string PasswordMatch = "New password can't be same as old password.";
        public const string PasswordCheck = "Please enter a valid old password.";
        public const string PasswordFieldValidation = "One or more fields are required.";
    }
}