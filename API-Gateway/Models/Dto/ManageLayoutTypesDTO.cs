namespace API_Gateway.Models.Dto
{
    public class ManageLayoutTypesDTO
    {
        public int Id { get; set; }
        public int LayoutId { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string? Options { get; set; }
        public string? ClassName { get; set; }
        public bool? HasInnerColumns { get; set; } = false;
        public int? Columns { get; set; } = 0;
        public string? MinImage { get; set; }
        public string? MaxImage { get; set; }
    }
}
