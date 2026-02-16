namespace API_Gateway.Models.Dto
{
    public class AdminSignUpForm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public int UserTypeId { get; set; }
        public string Password { get; set; }
        public string? ProfileImage { get; set; }
        public IFormFile? FileName { get; set; }
        public string? ReceiveNotifications { get; set; }
    }
}
