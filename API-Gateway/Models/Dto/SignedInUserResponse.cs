

namespace API_Gateway.Models.Dto
{
    public class SignedInUserResponse
    {
        public int? Code { get; set; }

        public string? Message { get; set; }

        public string? Response { get; set; }

        public UserSignInResponseModel? CurrentUser { get; set; }

        public TokenModel? Tokens { get; set; }
        public List<PageModuleSignInResponse>? PageAccess { get; set; }

    }
}
