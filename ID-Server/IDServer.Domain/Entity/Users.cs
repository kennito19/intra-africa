using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Domain.Entity
{
    public class Users : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfileImage { get; set; }
        public int? UserTypeId { get; set; }
        public RoleType? roleType { get; set; }
        public string? Status { get; set; }
        public string? Gender { get; set; }
        public int? MobileOTP { get; set; }
        public string? ReceiveNotifications { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? timestamp { get; set; }
        public string? AccountType { get; set; }

        public ICollection<UserSessions> userSessions { get; set; }
        public ICollection<AssignPageRole>? pageAccess { get; set; }
    }
}
