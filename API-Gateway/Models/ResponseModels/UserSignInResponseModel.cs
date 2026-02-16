using API_Gateway.Models.Entity.User;

namespace API_Gateway.Models.ResponseModels
{
    public class UserSignInResponseModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserType { get; set; }
        public string? Role { get; set; }

        public UserSignInResponseModel(Users user, IList<string> roles)
        {
            UserId = user.Id;
            UserName = user.Email;
            UserType = user.UserType;
            Role = roles.First();
        }

    }
}
