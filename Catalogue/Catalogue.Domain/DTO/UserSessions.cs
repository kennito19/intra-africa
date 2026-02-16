using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.DTO
{
    public class UserSessions
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public string DeviceId { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string AccessToken { get; set; }

        public DateTime LoggedOn { get; set; }
    }
}
