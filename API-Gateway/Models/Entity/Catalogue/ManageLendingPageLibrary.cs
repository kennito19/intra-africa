namespace API_Gateway.Models.Entity.Catalogue
{
    public class ManageLendingPageLibrary
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public int Sequence { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        public string? SearchText { get; set; }
    }
}
