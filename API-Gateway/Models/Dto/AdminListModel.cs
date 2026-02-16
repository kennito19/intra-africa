namespace API_Gateway.Models.Dto
{
    public class AdminListModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public string MobileNo { get; set; }
        public string ProfileImage { get; set; } = "https://www.shutterstock.com/image-vector/black-empty-set-null-void-260nw-2153177703.jpg";
        public string Status { get; set; } = "Active";
        public string? ReceiveNotifications { get; set; }
    }
}
