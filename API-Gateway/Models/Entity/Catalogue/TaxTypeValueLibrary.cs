namespace API_Gateway.Models.Entity.Catalogue
{
    public class TaxTypeValueLibrary
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int TaxTypeID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? TaxType { get; set; }
        public string? TaxTypeValue { get; set; }
    }
}
