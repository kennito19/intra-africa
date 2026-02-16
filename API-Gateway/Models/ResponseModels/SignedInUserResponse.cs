namespace API_Gateway.Models.ResponseModels
{
    public class SignedInUserResponse
    {
        public string? Response { get; set; }

        public UserSignInResponseModel? CurrentUser { get; set; }

        public TokenModel? Tokens { get; set; }

        public SignedInUserResponse(string response, UserSignInResponseModel user, TokenModel token)
        {

            Response = response;
            CurrentUser = user;
            Tokens = token;
        }


    }
}
