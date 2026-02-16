
namespace API_Gateway.Models.Dto
{
    public class UserSignInResponseModel
    {
        public string? UserId { get; set; }
        public string? FullName { get; set; }
        public string? ProfileImage { get; set; }
        public string? UserName { get; set; }
        public string? UserType { get; set; }
        public string? Role { get; set; }
        public string? Phone { get; set; }
        public string? Status { get; set; }
        public string? Gender { get; set; }
        public bool? IsPhoneConfirmed { get; set; }
        public bool? IsEmailConfirmed { get; set; }


    }
}
