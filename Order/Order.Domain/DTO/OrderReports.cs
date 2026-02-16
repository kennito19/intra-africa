using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.DTO
{
    public class OrderReports
    {
        public int RowNumber { get; set; }
        public int RecordCount { get; set; }
        public int? OrderItemId { get; set; }
        public int? OrderId { get; set; }
        public string? OrderNo { get; set; }
        public string? OrderBy { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? SellerId { get; set; }
        public int? BrandId { get; set; }
        public int? ProductId { get; set; }
        public string? ProductGUID { get; set; }
        public string? ProductName { get; set; }
        public int? SellerProductId { get; set; }
        public string? ProductSKUCode { get; set; }
        public decimal? MRP { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? Discount { get; set; }
        public int? Qty { get; set; }
        public bool? IsCouponApplied { get; set; }
        public string? Coupon { get; set; }
        public decimal? CoupontDiscount { get; set; }
        public decimal? ShippingCharge { get; set; }
        public string? ShippingChargePaidBy { get; set; }
        public string? ShippingZone { get; set; }
        public string? UserName { get; set; }
        public string? UserPhoneNo { get; set; }
        public string? UserEmail { get; set; }
        public string? UserAddressLine1 { get; set; }
        public string? UserAddressLine2 { get; set; }
        public string? UserLandmark { get; set; }
        public string? UserCity { get; set; }
        public string? UserState { get; set; }
        public string? UserPincode { get; set; }
        public string? orderStatus { get; set; }
        public string? Status { get; set; }
        public decimal? ItemTotalAmount { get; set; }
        public decimal? ItemSubTotal { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? PaymentMode { get; set; }
        public decimal? CODCharge { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
