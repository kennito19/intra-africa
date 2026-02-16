namespace API_Gateway.Models.Dto
{
    public class ManageLayoutOptionDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
