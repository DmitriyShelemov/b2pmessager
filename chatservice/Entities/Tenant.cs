using System.ComponentModel.DataAnnotations.Schema;

namespace chatservice.Entities
{
    [Table("Tenants")]
    public class Tenant
    {
        public Guid TenantUID { get; set; }

        public bool Deleted { get; set; }
    }
}
