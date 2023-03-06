using MicroOrm.Dapper.Repositories.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace messageservice.Entities
{
    [Table("Messages")]
    public class Message
    {
        [Key, Identity]
        public int MessageID { get; set; }

        public Guid MessageUID { get; set; }

        public Guid ChatUID { get; set; }

        public Guid TenantUID { get; set; }

        public string? MessageText { get; set; }

        public bool Deleted { get; set; }
    }
}
