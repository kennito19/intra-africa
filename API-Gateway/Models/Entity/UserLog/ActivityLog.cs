namespace API_Gateway.Models.Entity.UserLog
{
    public class ActivityLog
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int? Id { get; set; }
        public string? UserId { get; set; }
        public string UserType { get; set; }
        public string URL { get; set; }
        public string Action { get; set; }
        public string LogTitle { get; set; }
        public string LogDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Searchtext { get; set; }
    }
}
