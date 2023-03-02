using System.Text.Json.Serialization;

namespace facadeservice.Entities
{
    public class Chat
    {
        [JsonIgnore]
        public int ChatID { get; set; }

        public virtual Guid ChatUID { get; set; }

        [JsonIgnore]
        public Guid TenantUID { get; set; }

        public string? Name { get; set; }

        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}
