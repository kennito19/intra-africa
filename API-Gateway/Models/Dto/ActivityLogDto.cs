namespace API_Gateway.Models.Dto
{
    public class ActivityLogDto
    {
        public string? UserId { get; set; }
        public string UserType { get; set; }
        public string URL { get; set; }
        public string Action { get; set; }
        public string LogTitle { get; set; }
        public string LogDescription { get; set; }
    }
}
