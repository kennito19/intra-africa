namespace API_Gateway.Models.Entity.User
{
    public class KycCounts
    {
        public int Total { get; set; }
        public int Completed { get; set; }
        public int Pending { get; set; }
        public int NotApproved { get; set; }
    }
}
