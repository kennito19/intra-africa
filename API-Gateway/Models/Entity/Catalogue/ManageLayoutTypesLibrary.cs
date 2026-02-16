namespace API_Gateway.Models.Entity.Catalogue
{
    public class ManageLayoutTypesLibrary
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int LayoutId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string? Options { get; set; }
        public string? ClassName { get; set; }
        public bool? HasInnerColumns { get; set; } = false;
        public int? Columns { get; set; } = 0;
        public string? MinImage { get; set; }
        public string? MaxImage { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? Searchtext { get; set; }
        public string? LayoutName { get; set; }
    }
}
