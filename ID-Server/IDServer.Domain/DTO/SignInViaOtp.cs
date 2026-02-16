using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Domain.DTO
{
    public class SignInViaOtp
    {
        public string MobileNo { get; set; }
        public string otp { get; set; }
        public string DeviceId { get; set; }
    }

    public class GenerateMobileOtp
    {
        public string MobileNo { get; set; }
    }
}
