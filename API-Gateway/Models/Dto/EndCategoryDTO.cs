namespace API_Gateway.Models.Dto
{
    public class EndCategoryDTO
    {
        public int Id { get; set; }
        public string? Guid { get; set; }
        public string Name { get; set; }
        public int CurrentLevel { get; set; }
        public int? ParentId { get; set; }
        public string? Image { get; set; }
        public IFormFile? FileName { get; set; }
        public string? PathIds { get; set; }
        public string? PathNames { get; set; }
        public string? MetaTitles { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
        public string? Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? ParentName { get; set; }
        public string? ParentPathIds { get; set; }
        public string? ParentPathNames { get; set; }
    }
}
