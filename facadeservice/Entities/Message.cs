using System.Text.Json.Serialization;

namespace facadeservice.Entities
{
    public class Message
    {
        [JsonIgnore]
        public int MessageID { get; set; }

        public virtual Guid MessageUID { get; set; }

        public virtual Guid ChatUID { get; set; }

        [JsonIgnore]
        public Guid TenantUID { get; set; }

        public string? MessageText { get; set; }

        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}
