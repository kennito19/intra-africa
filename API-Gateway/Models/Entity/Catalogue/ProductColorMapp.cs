namespace API_Gateway.Models.Entity.Catalogue
{
    public class ProductColorMapp
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int? ProductID { get; set; }
        public int? ColorID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ColorName { get; set; }
        public string? ColorCode { get; set; }
    }

}
