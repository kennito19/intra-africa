

namespace API_Gateway.Models.Dto
{
    public class CustomerListModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string? ProfileImage { get; set; }
        public string? UserType { get; set; } = "Default";
        public string MobileNo { get; set; }
        public string? Gender { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneConfirmed { get; set; }
    }
}
