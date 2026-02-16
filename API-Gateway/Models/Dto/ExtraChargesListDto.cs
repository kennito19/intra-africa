using Newtonsoft.Json;

namespace API_Gateway.Models.Dto
{
    public class ExtraChargesListDto
    {
        [JsonProperty("customerPricing")]
        public List<CustomerPricing> CustomerPricing { get; set; }
    }

    public class CustomerPricing
    {
        [JsonProperty("local")]
        public Dictionary<string, OtherCharges> OtherCharges { get; set; }
    }

    public class OtherCharges
    {
        [JsonProperty("charges_paid_by")]
        public decimal ChargesPaidBy { get; set; }

        [JsonProperty("charges_in")]
        public decimal ChargesIn { get; set; }

        [JsonProperty("charges")]
        public decimal Charges { get; set; }

        [JsonProperty("actual_Charges")]
        public decimal ActualCharges { get; set; }
    }
}
