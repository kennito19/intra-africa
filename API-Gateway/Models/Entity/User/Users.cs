namespace API_Gateway.Models.Entity.User
{
    public class Users
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfileImage { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserType { get; set; }
        public string? Status { get; set; }
        public string? Gender { get; set; }
        public int? MobileOTP { get; set; }
        public string? ReceiveNotifications { get; set; }

        //public bool IsDeleted { get; set; } = false;
        //public int? CreatedBy { get; set; }
        //public DateTime? CreatedAt { get; set; }
        //public int? ModifiedBy { get; set; }
        //public DateTime? ModifiedAt { get; set; }
        //public string? DeletedBy { get; set; }
        //public DateTime? DeletedAt { get; set; }
        //public byte[]? timestamp { get; set; }
    }
}
