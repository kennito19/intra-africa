using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Domain.Entity
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

        public Users? user { get; set; }

        public UserSessions()
        {

        }
        public UserSessions(string? UID, string DID, string rt, DateTime? refExpire, string Token, DateTime loginTime)
        {

            UserId = UID;
            DeviceId = DID;
            RefreshToken = rt;
            RefreshTokenExpiryTime = refExpire;
            AccessToken = Token;
            LoggedOn = loginTime;

        }
    }
}
