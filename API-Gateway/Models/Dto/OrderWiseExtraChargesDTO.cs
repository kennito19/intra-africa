namespace API_Gateway.Models.Dto
{
    public class OrderWiseExtraChargesDTO
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int OrderItemID { get; set; }
        public string ChargesType { get; set; }
        public string ChargesPaidBy { get; set; }
        public string ChargesIn { get; set; }
        public decimal? ChargesValueInPercentage { get; set; }
        public decimal ChargesValueInAmount { get; set; }
        public decimal? ChargesMaxAmount { get; set; }
        public decimal TaxOnChargesAmount { get; set; }
        public decimal ChargesAmountWithoutTax { get; set; }
        public decimal TotalCharges { get; set; }
    }
}
