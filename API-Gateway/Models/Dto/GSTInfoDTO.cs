namespace API_Gateway.Models.Dto
{
    public class GSTInfoDTO
    {
        public int Id { get; set; }
        public string? UserID { get; set; }
        public string? GSTNo { get; set; }
        public string? LegalName { get; set; }
        public string? TradeName { get; set; }
        public string? GSTType { get; set; }
        public string? GSTDoc { get; set; }
        public string? RegisteredAddressLine1 { get; set; }
        public string? RegisteredAddressLine2 { get; set; }
        public string? RegisteredLandmark { get; set; }
        public string? RegisteredPincode { get; set; }
        public int? RegisteredStateId { get; set; }
        public int? RegisteredCityId { get; set; }
        public int? RegisteredCountryId { get; set; }
        public string? TCSNo { get; set; }
        public string? Status { get; set; }
        public bool IsHeadOffice { get; set; } = false;

        public IFormFile? FileName { get; set; }
    }
}
