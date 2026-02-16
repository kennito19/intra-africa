namespace API_Gateway.Models.Entity.Catalogue
{
    public class SpecificationLibrary
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int ID { get; set; }
        public string? Guid { get; set; }
        public string? Name { get; set; }
        public string? FieldType { get; set; }
        public int? ParentId { get; set; }
        public string? PathIds { get; set; }
        public string? PathName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool isDeleted { get; set; } = false;
        public bool IsChildParent { get; set; } = false;
        public string? ParentName { get; set; }
        public string? ParentPathIds { get; set; }
        public string? ParentPathNames { get; set; }
    }
}
