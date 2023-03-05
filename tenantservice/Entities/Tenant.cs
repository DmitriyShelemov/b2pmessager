using MicroOrm.Dapper.Repositories.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace tenantservice.Entities
{
    [Table("Tenants")]
    public class Tenant
    {
        [Key, Identity]
        public int TenantID { get; set; }

        public virtual Guid TenantUID { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public bool Deleted { get; set; }
    }
}
