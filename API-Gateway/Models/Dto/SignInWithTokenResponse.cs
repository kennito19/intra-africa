using API_Gateway.Helper;
using API_Gateway.Models.Entity.User;
using IdentityModel.Client;

namespace API_Gateway.Models.Dto
{
    public class SignInWithTokenResponse
    {
        public string Message { get; set; }
        public Users CurrentUser { get; set; }
        public TokenResponseClass tokenResponse { get; set; }

        public SignInWithTokenResponse(string msg, Users user, TokenResponse token)
        {
            Message = msg;
            CurrentUser = user;
            var baseResponse = new BaseResponse<TokenResponseClass>();
            tokenResponse = baseResponse.Convertor(token.Raw);
        }
    }
}
