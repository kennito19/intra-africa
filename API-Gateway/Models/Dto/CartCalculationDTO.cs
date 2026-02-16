namespace API_Gateway.Models.Dto
{
    public class CartCalculationDTO
    {
        public int? CartId { get; set; }
        public string? CartSessionId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public string? PaymentMode { get; set; }
        public string? Pincode { get; set; }

    }
}
