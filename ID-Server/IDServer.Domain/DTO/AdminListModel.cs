

namespace IDServer.Domain.DTO
{
    public class AdminListModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int UserTypeId { get; set; }
        public string? UserType { get; set; }
        public string MobileNo { get; set; }
        public string ProfileImage { get; set; }
        public string Status { get; set; } = "Active";
        public string? ReceiveNotifications { get; set; }
    }
}
