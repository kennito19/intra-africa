using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Domain.DTO
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
