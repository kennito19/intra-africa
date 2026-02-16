namespace API_Gateway.Models.Dto
{
    public class OrderPackageDtoList
    {
        public int? Id { get; set; }
        public string? PackageNo { get; set; }
        public string? OrderItemIds { get; set; }
        public int? TotalItems { get; set; }
        public int? NoOfPackage { get; set; }
        public decimal? PackageAmount { get; set; }
        public decimal? CodCharges { get; set; }
    }
}
