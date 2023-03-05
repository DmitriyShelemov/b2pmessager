using MicroOrm.Dapper.Repositories.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace chatservice.Entities
{
    [Table("Tenants")]
    public class Tenant
    {
        [Key, Identity]
        public virtual Guid TenantUID { get; set; }

        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}
