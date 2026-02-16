using API_Gateway.Models.Entity.Catalogue;

namespace API_Gateway.Models.Dto
{
    public class checkOfferDto
    {
        public ManageOffersLibrary offers { get; set; }
        public bool CouponApplied { get; set; } = false;
        public bool ValidOnlyOnline { get; set; } = false;
        public string message { get; set; }
    }
}
