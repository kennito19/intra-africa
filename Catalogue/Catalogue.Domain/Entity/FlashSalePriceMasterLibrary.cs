using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
    public class FlashSalePriceMasterLibrary
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int SellerProductId { get; set; }
        public int SellerWiseProductPriceMasterId { get; set; }
        public int CollectionId { get; set; }
        public int CollectionMappingId { get; set; }
        public decimal? MRP { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? Discount { get; set; }
        public string? Status { get; set; }
        public bool? IsSellerOptIn { get; set; } = null;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; } = null;

        public string? SearchText { get; set; }
        public string? CollectionName { get; set; }
        public int? SizeID { get; set; } = null;
        public string? SizeName { get; set; } = null;

    }
}
