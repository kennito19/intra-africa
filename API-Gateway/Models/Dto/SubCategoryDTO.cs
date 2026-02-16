namespace API_Gateway.Models.Dto
{
    public class SubCategoryDTO
    {
        public int? Id { get; set; }
        public string? Guid { get; set; }
        public string Name { get; set; }
        public int CurrentLevel { get; set; }
        public int ParentId { get; set; }
        public string? Image { get; set; }
        public IFormFile? FileName { get; set; }
        public string? MetaTitles { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
        public string? Status { get; set; }
        public string? Color { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }
        public bool IsImageAvailable { get; set; } = true;
    }
}
