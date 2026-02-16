namespace API_Gateway.Models.Entity.Order
{
    public class OrderItems
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }

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


        public string? ShippingZone { get; set; }
        public decimal? ShippingCharge { get; set; }
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



        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? OrderBy { get; set; }
        public string? OrderNo { get; set; }
        public string? OrderStatus { get; set; }
        public decimal? OrderCodCharges { get; set; }
        public string? ColorName { get; set; }
        public string? ProductImage { get; set; }
        public string? ExtraDetails { get; set; }
    }
}
