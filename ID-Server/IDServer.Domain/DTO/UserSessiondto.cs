using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Domain.DTO
{
    public class UserSessiondto
    {
        public string DeviceId { get; set; }

        public string AccessToken { get; set; }
    }
}
