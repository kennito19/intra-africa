using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Domain.Entity
{
    public class UserClaimResponse
    {
        public string UserName { get; set; }

        public string UserId { get; set; }
        public List<string>? Roles { get; set; }


        public List<UserAccessClaims> claims { get; set; }

        public UserClaimResponse(string User, string Id, List<string> RoleNames, List<UserAccessClaims> claimList)
        {
            UserName = User;
            UserId = Id;
            Roles = RoleNames;
            claims = claimList;
        }
    }
}
