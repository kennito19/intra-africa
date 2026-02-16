namespace API_Gateway.Models.Entity.Catalogue
{
    public class AssignTaxRateToHSNCode
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int HsnCodeId { get; set; }
        public int TaxValueId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        public string? HsnCode { get; set; }
        public string? TaxType { get; set; }
        public string? TaxName { get; set; }
        public string? TaxTypeValue { get; set; }

        public string? SearchText { get; set; }
        public string? DisplayName { get; set; }
    }
}
