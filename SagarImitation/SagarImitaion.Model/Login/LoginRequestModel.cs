using SagarImitation.Model.Common;
using System.ComponentModel.DataAnnotations;

namespace SagarImitation.Model.Login
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Email id required!")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Password required!")]
        public string Password { get; set; }
    }

    public class SaltResponseModel : ResponseModel
    {
        public string? PasswordSalt { get; set; }
        public string? Password { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ForgetPasswordRequestModel
    {
        [Required(ErrorMessage = "EmailId is required")]
        public required string EmailId { get; set; }
    }

    public class VerificationOTPRequestModel
    {
        [Required(ErrorMessage = "Email id required!")]
        public required string EmailId { get; set; }

        [Required(ErrorMessage = "Verification-code is required!")]
        public required int OTP { get; set; }
    }

    public class UserForgotPasswordModel
    {
        public string? UserId { get; set; }
        public string? EncryptedURLDateTime { get; set; }
    }
}