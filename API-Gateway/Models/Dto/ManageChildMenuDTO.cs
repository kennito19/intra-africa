namespace API_Gateway.Models.Dto
{
    public class ManageChildMenuDTO
    {
        public int Id { get; set; }
        public string MenuType { get; set; }
        public int HeaderId { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImageAlt { get; set; }
        public bool HasLink { get; set; }
        public string? RedirectTo { get; set; }
        public int? LendingPageId { get; set; }
        public int? CategoryId { get; set; }
        public int? StaticPageId { get; set; }
        public int? CollectionId { get; set; }
        public string? CustomLink { get; set; }
        public string? Sizes { get; set; }
        public string? Colors { get; set; }
        public string? Specifications { get; set; }
        public string? Brands { get; set; }
        public int Sequence { get; set; }

        public bool IsImageAvailable { get; set; } = true;
    }
}
