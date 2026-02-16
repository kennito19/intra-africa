namespace API_Gateway.Models.Entity.IDServer
{
    public class UserSessions
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public string DeviceId { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string AccessToken { get; set; }

        public DateTime LoggedOn { get; set; }
    }
}
