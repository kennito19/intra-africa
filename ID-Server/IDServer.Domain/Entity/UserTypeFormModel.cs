using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IDServer.Domain.Entity
{
    public class UserTypeFormModel
    {
        public int? UserTypeId { get; set; }
        public string UserTypeName { get; set; }

        public List<AssignPageRole>? pagesAssigned { get; set; }
    }
}
