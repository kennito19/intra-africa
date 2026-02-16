namespace API_Gateway.Models.Dto
{
    public class ManageLayoutsDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
