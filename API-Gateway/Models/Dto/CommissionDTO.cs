namespace API_Gateway.Models.Dto
{
    public class CommissionDTO
    {
        public int ID { get; set; }
        public int? CatID { get; set; }
        public string? SellerID { get; set; }
        public int? BrandID { get; set; }
        public decimal AmountValue { get; set; }
    }
}
