using System.Text.Json.Serialization;

namespace API_Gateway.Models.Entity.Catalogue
{
    public class ManageOffersLibrary
    {
        [JsonIgnore]
        public int RowNumber { get; set; }
        [JsonIgnore]
        public int PageCount { get; set; }
        [JsonIgnore]
        public int RecordCount { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string? terms { get; set; }
        public string? offerCreatedBy { get; set; }
        public string? offerType { get; set; }
        public string? usesType { get; set; }
        public string? usesPerCustomer { get; set; }
        public decimal? value { get; set; } = null;
        public decimal? minimumOrderValue { get; set; } = null;
        public decimal? maximumDiscountAmount { get; set; } = null;
        public int? buyQty { get; set; } = null;
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
        public bool? OfferStatus { get; set; } = null;
        public string? Searchtext { get; set; }
        public decimal? totalsales { get; set; }
        public decimal? totalused { get; set; }
        public List<ManageOffersMapping>? offerItems { get; set; }
    }
}
