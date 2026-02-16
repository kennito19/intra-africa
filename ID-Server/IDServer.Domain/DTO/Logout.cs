using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Domain.DTO
{
    public class Logout
    {
        public string UserId { get; set; }
        public string Deviceid { get; set; }
        public string RefreshToken { get; set; }
    }
}
