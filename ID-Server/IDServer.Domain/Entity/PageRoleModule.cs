using System.Text.Json.Serialization;


namespace IDServer.Domain.Entity
{
    public class PageRoleModule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public string URL { get; set; }

        [JsonIgnore]
        public ICollection<AssignPageRole>? assignPages { get; set; }
    }
}
