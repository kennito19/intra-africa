namespace API_Gateway.Models.Dto
{
    public class Logout
    {
        public string UserId { get; set; }
        public string Deviceid { get; set; }
        public string RefreshToken { get; set; }
    }
}
