namespace API_Gateway.Models.Entity.IDServer
{
    public class UserTypeFormModel
    {
        public int? UserTypeId { get; set; }
        public string UserTypeName { get; set; }

        public List<AssignPageRole>? pagesAssigned { get; set; }
    }
}
