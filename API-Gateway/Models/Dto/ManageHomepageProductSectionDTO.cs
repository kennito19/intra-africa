namespace API_Gateway.Models.Dto
{
    public class ManageHomepageProductSectionDTO
    {

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
        public string? ListType { get; set; } = null;
        public int? TopProducts { get; set; } = null;
        public int? CategoryId { get; set; } = null;
        public string Status { get; set; }
        public string? BackgroundColor { get; set; } = null;
        public bool? InContainer { get; set; } = null;
        public string? TitleColor { get; set; } = null;
        public string? TextColor { get; set; } = null;

        public IEnumerable<productSection>? productSections { get; set; }


    }
    public class productSection
    {
        public string? productId { get; set; }
        public string? AssignCity { get; set; }
        public string? AssignState { get; set; }
        public string? AssignCountry { get; set; }
    }
}
