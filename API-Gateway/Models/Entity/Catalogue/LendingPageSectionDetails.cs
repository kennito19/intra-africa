namespace API_Gateway.Models.Entity.Catalogue
{
    public class LendingPageSectionDetails
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int LendingPageSectionId { get; set; }
        public int? LayoutTypeDetailsId { get; set; }
        public int? OptionId { get; set; }
        public string? Image { get; set; }
        public string? ImageAlt { get; set; }
        public bool? IsTitleVisible { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? TitlePosition { get; set; }
        public int Sequence { get; set; }
        public int? Columns { get; set; }
        public string? RedirectTo { get; set; }
        public int? CategoryId { get; set; }
        public string? BrandIds { get; set; }
        public string? SizeIds { get; set; }
        public string? SpecificationIds { get; set; }
        public string? ColorIds { get; set; }
        public int? CollectionId { get; set; }
        public string? ProductId { get; set; }
        public int? StaticPageId { get; set; }
        public string? CustomLinks { get; set; }
        public string? TitleColor { get; set; }
        public string? SubTitleColor { get; set; }
        public string? TitleSize { get; set; }
        public string? SubTitleSize { get; set; }
        public bool? ItalicSubTitle { get; set; }
        public bool? ItalicTitle { get; set; }
        public string? Description { get; set; }
        public string? SliderType { get; set; }
        public string? VideoLinkType { get; set; }
        public string? VideoId { get; set; }
        public string? Name { get; set; }
        public string? AssignCity { get; set; }
        public string? AssignState { get; set; }
        public string? AssignCountry { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        public string? SearchText { get; set; }
        public string? LendingPageSectionName { get; set; }
        public string? OptionName { get; set; }
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryPathName { get; set; }
    }
}
