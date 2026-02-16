using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Domain.Entity
{
    public class SignUp
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string EmailID { get; set; }

        [Required]
        public string MobileNo { get; set; }
        public int? UserTypeId { get; set; }
        public string? ProfileImage { get; set; }
        public string? Gender { get; set; }
        public string? ReceiveNotifications { get; set; }
        public string? DeviceId { get; set; }


        [Required]
        public string Password { get; set; }
    }
}
