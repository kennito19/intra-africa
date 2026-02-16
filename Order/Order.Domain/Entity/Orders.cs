using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entity
{
    public class Orders
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }



        public int Id { get; set; }
        public string? Guid { get; set; }
        public string OrderNo { get; set; }
        public string? SellerID { get; set; } = null;
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPhoneNo { get; set; }
        public string UserEmail { get; set; }
        public string UserAddressLine1 { get; set; }
        public string? UserAddressLine2 { get; set; }
        public string? UserLandmark { get; set; }
        public int UserPincode { get; set; }
        public string? UserCity { get; set; }
        public string? UserState { get; set; }
        public string? UserCountry { get; set; }
        public string? UserGSTNo { get; set; }
        public string? PaymentMode { get; set; }
        public decimal TotalShippingCharge { get; set; }
        public decimal TotalExtraCharges { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCouponApplied { get; set; }
        public string? Coupon { get; set; }
        public decimal? CoupontDiscount { get; set; }
        public string? CoupontDetails { get; set; }
        public decimal? CODCharge { get; set; }
        public decimal PaidAmount { get; set; }
        public bool IsSale { get; set; } = false;
        public string? SaleType { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? Status { get; set; }
        public string? PaymentInfo { get; set; }
        public string? OrderBy { get; set; }
        public bool? IsRetailer { get; set; }
        public bool? IsVertualRetailer { get; set; }
        public bool? IsReplace { get; set; }
        public int? ParentID { get; set; }


        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? SearchText { get; set; }
        public bool? NotInStatus { get; set; }
        public string? OrderReferenceNo { get; set; }

        public string? FromDate { get; set; }
        public string? ToDate { get; set; }

    }
}
