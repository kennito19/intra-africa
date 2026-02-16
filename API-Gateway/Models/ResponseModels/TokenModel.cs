namespace API_Gateway.Models.ResponseModels
{
    public class TokenModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        public TokenModel()
        {

        }
        public TokenModel(string accesstk, string refreshtk)
        {
            AccessToken = accesstk;
            RefreshToken = refreshtk;
        }
    }
}
