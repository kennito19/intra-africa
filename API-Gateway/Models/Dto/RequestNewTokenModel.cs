namespace API_Gateway.Models.Dto
{
    public class RequestNewTokenModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string DeviceId { get; set; }
        public string UserId { get; set; }

    }
}
