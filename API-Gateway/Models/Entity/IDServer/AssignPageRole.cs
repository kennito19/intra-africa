using API_Gateway.Models.Entity.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API_Gateway.Models.Entity.IDServer
{
    public class AssignPageRole
    {
        public int Id { get; set; }

        public int PageRoleId { get; set; }

        [NotMapped]
        public string? PageRoleName { get; set; }

        [JsonIgnore]
        public PageRoleModule? Page { get; set; }

        public int? RoleTypeId { get; set; }

        [JsonIgnore]
        public RoleType? RoleType { get; set; }

        public string? UserID { get; set; }

        [JsonIgnore]
        public Users? User { get; set; }



        public bool Read { get; set; }
        public bool Add { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
    }
}
