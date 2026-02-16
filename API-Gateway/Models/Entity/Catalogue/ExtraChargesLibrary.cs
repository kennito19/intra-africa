namespace API_Gateway.Models.Entity.Catalogue
{
    public class ExtraChargesLibrary
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int? CatID { get; set; }
        public int? ChargesPaidByID { get; set; }
        public string Name { get; set; }
        public string? ChargesOn { get; set; }
        public bool? IsCompulsary { get; set; }
        public string? ChargesIn { get; set; }
        public decimal PercentageValue { get; set; }
        public decimal AmountValue { get; set; }
        public decimal? MaxAmountValue { get; set; } = null;
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? CategoryName { get; set; }
        public string? ChargesPaidByName { get; set; }
    }
}
