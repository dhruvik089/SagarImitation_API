using System.ComponentModel.DataAnnotations;

namespace SagarImitation.Model.User
{
    public class UserRequestModel
    {
        public long UserId { get; set; }

        [Required(ErrorMessage = "FirstName is required.")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Email Id is required.")]
        public required string EmailId { get; set; }

        public string? Password { get; set; }
        //[Required]
        //[RegularExpression(@"^[0-9]+$", ErrorMessage = "MobileNo must contain only numbers.")]
        //[StringLength(10, ErrorMessage = "MobileNo must be at least 10 characters long.", MinimumLength = 10)]
        //public required string MobileNo { get; set; }
    }
}