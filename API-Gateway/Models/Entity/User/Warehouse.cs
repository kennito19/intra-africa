namespace API_Gateway.Models.Entity.User
{
    public class Warehouse
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public string? UserID { get; set; }
        public int? GSTInfoId { get; set; }
        public string? Name { get; set; }
        public string? ContactPersonName { get; set; }
        public string? ContactPersonMobileNo { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Landmark { get; set; }
        public string? Pincode { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? CountryId { get; set; }
        public string? Status { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? CountryName { get; set; }
        public string? StateName { get; set; }
        public string? CityName { get; set; }
        public string? GSTNo { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfileImage { get; set; }
        public string? UserStatus { get; set; }
        public string? Email { get; set; }
    }
}
