using System.Text.Json.Serialization;

namespace facadeservice.Entities
{
    public class Tenant
    {
        [JsonIgnore]
        public int TenantID { get; set; }

        public virtual Guid TenantUID { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}
