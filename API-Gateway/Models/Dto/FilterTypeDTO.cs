namespace API_Gateway.Models.Dto
{
    public class FilterTypeDTO
    {
        public int? FilterTypeId { get; set; }
        public string? FilterTypeName { get; set; }
        public List<FilterValueDTO>? FilterValues { get; set; }
    }
}
