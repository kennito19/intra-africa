namespace API_Gateway.Models.Entity.Catalogue
{
    public class ProductImages
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int? ProductID { get; set; }
        public int? Sequence { get; set; }
        public string? Url { get; set; }
        public string? Type { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

    }
}
