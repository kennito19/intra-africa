namespace API_Gateway.Models.Dto
{
    public class ManageHeaderMenuDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImageAlt { get; set; }
        public bool HasLink { get; set; }
        public string? RedirectTo { get; set; }
        public int LendingPageId { get; set; }
        public int CategoryId { get; set; }
        public int StaticPageId { get; set; }
        public int CollectionId { get; set; }
        public string? CustomLink { get; set; }
        public int Sequence { get; set; }
        public string? color { get; set; }
        public bool IsImageAvailable { get; set; } = true;
    }
}
