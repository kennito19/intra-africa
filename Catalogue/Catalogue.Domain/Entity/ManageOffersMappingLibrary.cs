using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
	public class ManageOffersMappingLibrary
	{
		public int RowNumber { get; set; }
		public int PageCount { get; set; }
		public int RecordCount { get; set; }
        public int id { get; set; }
        public int offerId { get; set; }
        public int? categoryId { get; set; } = null;
        public string? sellerId { get; set; } = null;
        public int? brandId { get; set; } = null;
        public int? productId { get; set; } = null;
        public int? getProductId { get; set; } = null;
        public string? userId { get; set; } = null;
        public string? getDiscountType { get; set; } = null;
        public decimal? getDiscountValue { get; set; } = null;
        public decimal? getProductPrice { get; set; } = null;
        public bool? sellerOptIn { get; set; } = false;
        public string? optInSellerIds { get; set; } = null;
        public string? status { get; set; } = null;
        public string? ExtraDetails { get; set; } = null;
        public string? CategoryIds { get; set; } = null;
        public string? SellerIds { get; set; } = null;
        public string? Brandids { get; set; } = null;
        public string? ProductIds { get; set; } = null;
        public string? CreatedBy { get; set; }
		public DateTime? CreatedAt { get; set; }
		public string? ModifiedBy { get; set; }
		public DateTime? ModifiedAt { get; set; }
		public string? DeletedBy { get; set; }
		public DateTime? DeletedAt { get; set; }
		public bool IsDeleted { get; set; } = false;
		public string? Searchtext { get; set; }
		public string? offerName { get; set; }
		public string? productName { get; set; }
		public string? categoryName { get; set; }
		public string? categoryPathNames { get; set; }
        public string? offerIds { get; set; }
        public string? ApplyOn { get; set; }
    }
}
