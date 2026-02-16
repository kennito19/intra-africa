namespace API_Gateway.Models.Dto
{
    public class GuestUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public int? UserTypeId { get; set; }
        public string? Gender { get; set; }
        public string? DeviceId { get; set; }

    }
}
