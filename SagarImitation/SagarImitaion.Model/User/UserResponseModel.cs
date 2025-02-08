namespace SagarImitation.Model.User
{
    public class UserResponseModel
    {
        public long UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailId { get; set; }
        public string? MobileNo { get; set; }
        public string? Photo { get; set; }
        public long TotalRecords { get; set; }
        public long RowNumber { get; set; }
        public bool IsActive { get; set; }
    }
}