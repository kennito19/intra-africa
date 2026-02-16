namespace API_Gateway.Models.Dto
{
    public class ManageLayoutTypesDetailsDTO
    {
        public int Id { get; set; }
        public int LayoutId { get; set; }
        public int LayoutTypeId { get; set; }
        public string Name { get; set; }
        public string? SectionType { get; set; }
        public string? InnerColumns { get; set; }
    }
}
