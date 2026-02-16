using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Domain.Entity
{
    public class UserAccessClaims
    {
        public string? Resource { get; set; }
        public List<string>? AccessType { get; set; }
    }
}
