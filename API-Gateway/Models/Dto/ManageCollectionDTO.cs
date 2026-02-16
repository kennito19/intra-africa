namespace API_Gateway.Models.Dto
{
    public class ManageCollectionDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SubTitle { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public int Sequence { get; set; }
    }
}
