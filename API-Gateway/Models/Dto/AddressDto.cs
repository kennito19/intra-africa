namespace API_Gateway.Models.Dto
{
    public class AddressDto
    {
        public int? Id { get; set; }
        public string? UserId { get; set; }
        public string? AddressType { get; set; }
        public string? FullName { get; set; }
        public string? MobileNo { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Landmark { get; set; }
        public string? Pincode { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? CountryId { get; set; }
        public string? GSTNo { get; set; }
        public string? Status { get; set; }
        public bool SetDefault { get; set; } = false;
    }
    public class SetDefaultAddressDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool SetDefault { get; set; } = false;

    }
    public class UpdateStatusAddressDto
    {
        public int Id { get; set; }
        public string? Status { get; set; }
    }
}
