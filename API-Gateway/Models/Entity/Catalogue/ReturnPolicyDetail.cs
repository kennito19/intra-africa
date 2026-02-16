namespace API_Gateway.Models.Entity.Catalogue
{
    public class ReturnPolicyDetail
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int ReturnPolicyID { get; set; }
        public int ValidityDays { get; set; }
        public string? Title { get; set; }
        public string? Covers { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? ReturnPolicy { get; set; }
    }
}
