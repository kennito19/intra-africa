namespace API_Gateway.Models.Entity.Catalogue
{
    public class ManageHomePageSectionsLibrary
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int LayoutId { get; set; }
        public int LayoutTypeId { get; set; }
        public string Name { get; set; }
        public int? Sequence { get; set; }
        public int? SectionColumns { get; set; }
        public bool? IsTitleVisible { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? TitlePosition { get; set; }
        public string? LinkIn { get; set; }
        public string? LinkText { get; set; }
        public string? Link { get; set; }
        public string? LinkPosition { get; set; }
        public string Status { get; set; }
        public string? ListType { get; set; } = null;
        public int? TopProducts { get; set; } = null;
        public int? TotalRowsInSection { get; set; } = null;
        public bool? IsCustomGrid { get; set; } = null;
        public int? NumberOfImages { get; set; } = null;
        public int? Column1 { get; set; } = null;
        public int? Column2 { get; set; } = null;
        public int? Column3 { get; set; } = null;
        public int? Column4 { get; set; } = null;
        public int? CategoryId { get; set; } = null;
        public string? BackgroundColor { get; set; } = null;
        public bool? InContainer { get; set; } = null;
        public string? TitleColor { get; set; } = null;
        public string? TextColor { get; set; } = null;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public string? LayoutTypeName { get; set; }
        public string? LayoutName { get; set; }
        public string? SearchText { get; set; }
    }



}
