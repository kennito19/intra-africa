using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Entity
{
    public class Wishlist
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? ProductId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserStatus { get; set; }
        public string? ProfileImage { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public bool? IsPhoneConfirmed { get; set; }
    }
}
