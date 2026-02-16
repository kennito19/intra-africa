using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.DTO
{
    public class GetCheckOutDetailsList
    {
        public int? RowNumber { get; set; }
        public int? PageCount { get; set; }
        public int? RecordCount { get; set; }
        public int? Id { get; set; }
        public string? UserId { get; set; }
        public string? SessionId { get; set; }
        public int? SellerProductMasterId { get; set; }
        public int? SizeId { get; set; }
        public int? Quantity { get; set; }
        public int? WarrantyId { get; set; }
        public decimal? TempMRP { get; set; }
        public decimal? TempSellingPrice { get; set; }
        public decimal? TempDiscount { get; set; }
        public decimal? SubTotal { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? Type { get; set; }
        public string? Fullname { get; set; }
        public string? Username { get; set; }
        public string? Phone { get; set; }
        public string? UserStatus { get; set; }


        public int? ProductId { get; set; }
        public string? ProductGuid { get; set; }
        public int? SellerProductId { get; set; }
        public int? ProductPricemasterId { get; set; }
        public string? ProductName { get; set; }
        public string? CustomeProductName { get; set; }
        public string? ProductSkuCode { get; set; }
        public string? SellerSkuCode { get; set; }
        public decimal? MRP { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? Discount { get; set; }
        public string? MarginIn { get; set; }
        public decimal? MarginCost { get; set; }
        public decimal? MarginPercentage { get; set; }
        public decimal? ItemMRP { get; set; }
        public decimal? ItemSellingPrice { get; set; }
        public decimal? ItemDiscount { get; set; }
        public int? TotalQty { get; set; }
        public int? MOQ { get; set; }
        public string? Image { get; set; }
        public string? Color { get; set; }
        public int? Categoryid { get; set; }
        public int? HSNCodeId { get; set; }
        public string? HSNCode { get; set; }
        public int? TaxValueId { get; set; }
        public string? TaxRate { get; set; }
        public string? SellerId { get; set; }
        public int? BrandId { get; set; }
        public int? WeightSlabId { get; set; }
        public string? WeightSlab { get; set; }
        public string? ProductStatus { get; set; }
        public string? Size { get; set; }
        public bool? LiveStatus { get; set; }
        public int? FlashSaleId { get; set; }
        public string? ExtraDetails { get; set; }
        public string? TierPriceList { get; set; }
        public string? ExtendedWarrantyList { get; set; }
        public string? ParentCategoryList { get; set; }
    }

    public class getChekoutCalculation
    {
        public string? CartJson { get; set; }
    } 
}
