using API_Gateway.Models.Dto;

namespace API_Gateway.Models.Entity.User
{
    public class Wishlist
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public UserProductsDTO? Products { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfileImage { get; set; }
        public string? UserStatus { get; set; }
        public string? Email { get; set; }
    }
}
