namespace API_Gateway.Models.Entity.Catalogue
{
    public class AssignReturnPolicyToCatagoryLibrary
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int? ReturnPolicyDetailID { get; set; }
        public int? CategoryID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? Category { get; set; }
        public int? ParentCategoryID { get; set; }
        public string? PathIds { get; set; }
        public string? PathNames { get; set; }
        public int? ReturnPolicyID { get; set; }
        public int? ValidityDays { get; set; }
        public string? Title { get; set; }
        public string? Covers { get; set; }
        public string? Description { get; set; }
        public string? ReturnPolicy { get; set; }
    }
}
