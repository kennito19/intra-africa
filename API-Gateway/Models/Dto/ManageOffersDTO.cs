namespace API_Gateway.Models.Dto
{
    public class ManageOffersDTO
    {
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
        public int? buyQty { get; set; } = null;
        public int? getQty { get; set; } = null;
        public string? applyOn { get; set; } = null;
        public bool? hasShippingFree { get; set; } = false;
        public bool? showToCustomer { get; set; } = false;
        public bool? onlyForOnlinePayments { get; set; } = false;
        public bool? onlyForNewCustomers { get; set; } = false;
        public string? startDate { get; set; }
        public string? startTime { get; set; }
        public string? endDate { get; set; }
        public string? endTime { get; set; }
        public string? status { get; set; }

        public List<ManageOffersMappingDTO>? offerItems { get; set; }
    }
}
