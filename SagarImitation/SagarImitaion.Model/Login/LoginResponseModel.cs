using SagarImitation.Model.Common;
using System.ComponentModel.DataAnnotations;

namespace SagarImitation.Model.Login
{
    public class LoginResponseModel
    {
         public required string JWTToken { get; set; }
    }

    public class LoginDetailModel : ResponseModel
    {
        public long UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }    
        public string? JWTToken { get; set; }
        //public bool IsAdmin { get; set; }
        public string? Photo { get; set;}
    }

    public class ForgetPasswordResponseModel
    {
        public long UserId { get; set; }
        public DateTime LastForgetPasswordSend { get; set; }
        public string? FullName { get; set; }
        public string? EmailId { get; set; }
    }

    public class ResetPasswordRequestModel
    {
        [Required(ErrorMessage = "The Create Password is required.")]
		[MaxLength(15, ErrorMessage = "Maxlength is 15 characters.")]
		public required string NewPassword { get; set; }

        [Required(ErrorMessage = "The Confirm Password is required.")]
		[MaxLength(15, ErrorMessage = "Maxlength is 15 characters.")]
		public required string ConfirmPassword { get; set; }
    }

    public class ChangePasswordRequestModel
    {
        [Required(ErrorMessage = "The Old Password is required.")]
		[MaxLength(15, ErrorMessage = "Maxlength is 15 characters.")]
		public required string OldPassword { get; set; }

		[MaxLength(15, ErrorMessage = "Maxlength is 15 characters.")]
		[Required(ErrorMessage = "The Create Password is required.")]
        public required string CreatePassword { get; set; }

        [Required(ErrorMessage = "The Confirm Password is required.")]
		[MaxLength(15, ErrorMessage = "Maxlength is 15 characters.")]
		public required string ConfirmPassword { get; set; }
    }

    public class ResetPasswordWithLinkRequestModel
    {
        [Required(ErrorMessage = "User Id required!")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "The Create Password is required.")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "The Confirm Password is required.")]
        public string ConfirmPassword { get; set; }
    }
}