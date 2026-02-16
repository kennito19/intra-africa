namespace API_Gateway.Models.Dto
{
    public class MasterProductDTO
    {
        public int? ProductId { get; set; }
        public string? ProductGuid { get; set; }
        public int? CategoryId { get; set; }
        public int? AssiSpecid { get; set; }
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryPathName { get; set; }
    }
}
