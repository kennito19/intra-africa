using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class OrderDetails
    {
        #region Order
        public int? OrderId { get; set; }
        public string OrderNo { get; set; }
        public string? SellerID { get; set; } = null;
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPhoneNo { get; set; }
        public string UserEmail { get; set; }
        public string UserAddressLine1 { get; set; }
        public string UserAddressLine2 { get; set; }
        public string? UserLendMark { get; set; }
        public string UserPincode { get; set; }
        public string UserCity { get; set; }
        public string UserState { get; set; }
        public string UserCountry { get; set; }
        public string UserGSTNo { get; set; }
        public string PaymentMode { get; set; }
        [JsonIgnore]
        public decimal TotalShippingCharge { get; set; } = 0;
        [JsonIgnore]
        public decimal TotalExtraCharges { get; set; } = 0;
        public decimal TotalAmount { get; set; } = 0;
        public bool? IsCouponApplied { get; set; }
        public string Coupon { get; set; }
        public decimal? CoupontDiscount { get; set; } = 0;
        public string? CoupontDetails { get; set; }
        public decimal? CODCharge { get; set; } = 0;
        public decimal PaidAmount { get; set; } = 0;
        public bool IsSale { get; set; }
        public string SaleType { get; set; }
        public DateTime? OrderDate { get; set; } = null;
        public DateTime? DeliveryDate { get; set; } = null;
        public int Deliverydays { get; set; } = 0;
        public string Status { get; set; }
        public string? PaymentInfo { get; set; }
        public string OrderBy { get; set; }
        public bool? IsRetailer { get; set; }
        public bool? IsVertualRetailer { get; set; }
        public bool? IsReplace { get; set; }
        public int? ParentId { get; set; }
        public string? OrderReferenceNo { get; set; }

        public int? RowNumber { get; set; }
        public int? PageCount { get; set; }
        public int? RecordCount { get; set; }
        #endregion

        public IEnumerable<OrderItemDTO> OrderItems { get; set; }

    }
}
