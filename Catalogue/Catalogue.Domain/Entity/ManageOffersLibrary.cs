using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
	public class ManageOffersLibrary
	{
		public int RowNumber { get; set; }
		public int PageCount { get; set; }
		public int RecordCount { get; set; }
        public int id { get; set; }
        public string? name { get; set; }
		public string? code { get; set; }
		public string? terms { get; set; }
		public string? offerCreatedBy { get; set; }
		public string? offerType { get; set; }
        public string? usesType { get; set; }
        public string? usesPerCustomer { get; set; }
		public decimal? value { get; set; } = null;
        public decimal? minimumOrderValue { get; set; } = null;
        public decimal? maximumDiscountAmount { get; set; } = null;
        [JsonIgnore]
		public int? buyQty { get; set; } = null;
        [JsonIgnore]
		public int? getQty { get; set; } = null;
        public string? applyOn { get; set; } = null;
        public bool? hasShippingFree { get; set; } = false;
		public bool? showToCustomer { get; set; } = false;
        public bool? onlyForOnlinePayments { get; set; } = false;
        public bool? onlyForNewCustomers { get; set; } = false;
        public DateTime? startDate { get; set; }
		public DateTime? endDate { get; set; }
		public string? status { get; set; }
		public string? CreatedBy { get; set; }
		public DateTime? CreatedAt { get; set; }
		public string? ModifiedBy { get; set; }
		public DateTime? ModifiedAt { get; set; }
		public string? DeletedBy { get; set; }
		public DateTime? DeletedAt { get; set; }
		public bool IsDeleted { get; set; } = false;
		public string? Searchtext { get; set; }
		public string? offerIds { get; set; }
		public bool? OfferStatus { get; set; } = null;
	}
}
