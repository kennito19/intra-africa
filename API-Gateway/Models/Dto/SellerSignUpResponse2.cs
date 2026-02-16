namespace API_Gateway.Models.Dto
{
    public class SellerSignUpResponse2
    {
        public string EmailID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int UserTypeId { get; set; }
        public string Password { get; set; }
        public string? ProfileImage { get; set; }
    }
}
