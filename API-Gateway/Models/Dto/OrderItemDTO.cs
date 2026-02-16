using API_Gateway.Models.Entity.Order;


using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
        public string? Guid { get; set; }
        public string SubOrderNo { get; set; }
        public string? SellerID { get; set; }
        public int BrandID { get; set; }
        public int CategoryId { get; set; }
        public int ProductID { get; set; }
        public string? ProductGUID { get; set; }
        public int SellerProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductSKUCode { get; set; }
        public decimal MRP { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Discount { get; set; }
        public int Qty { get; set; }
        public decimal TotalAmount { get; set; }
        public int? PriceTypeID { get; set; }
        public string? PriceType { get; set; }
        public int? SizeID { get; set; }
        public string? SizeValue { get; set; }
        public bool? IsCouponApplied { get; set; }
        public string? Coupon { get; set; }
        public decimal? CoupontDiscount { get; set; }
        public string? CoupontDetails { get; set; }

        [JsonIgnore]
        public string? ShippingZone { get; set; }
        [JsonIgnore]
        public decimal? ShippingCharge { get; set; }
        [JsonIgnore]
        public string? ShippingChargePaidBy { get; set; }


        public decimal SubTotal { get; set; }
        public string Status { get; set; }
        public int? WherehouseId { get; set; }
        public bool? IsReplace { get; set; }
        public int? ParentID { get; set; }

        public string? ReturnPolicyName { get; set; }
        public string? ReturnPolicyTitle { get; set; }
        public string? ReturnPolicyCovers { get; set; }
        public string? ReturnPolicyDescription { get; set; }
        public int? ReturnValidDays { get; set; }
        public DateTime? ReturnValidTillDate { get; set; }
        public string? BrandName { get; set; }
        public string? ProductImage { get; set; }
        public string? Color { get; set; }
        public string? SellerName { get; set; }
        public string? SellerPhoneNo { get; set; }
        public string? SellerEmailId { get; set; }
        public string? SellerStatus { get; set; }
        public string? SellerKycStatus { get; set; }

        public int? PackageId { get; set; }
        public string? PackageNo { get; set; }
        public string? PackageItemIds { get; set; }
        public int? TotalPakedItems { get; set; }
        public int? NoOfPackage { get; set; }
        public decimal? PackageAmount { get; set; }
        public decimal? PackageCodCharges { get; set; }
        public string? ShippmentBy { get; set; }
        public string? ExtraDetails { get; set; }
        public IEnumerable<OrderTaxInfoDTO> OrderTaxInfos { get; set; }

        public IEnumerable<OrderWiseExtraChargesDTO> OrderWiseExtraCharges { get; set; }
    }
}
