namespace API_Gateway.Models.Entity.Catalogue
{
    public class ManageHomePage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
