using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IDServer.Domain.Entity
{
    public class RoleType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<AssignPageRole>? AssignedPages { get; set; }

        [JsonIgnore]
        public ICollection<Users>? Users { get; set; }
    }
}
