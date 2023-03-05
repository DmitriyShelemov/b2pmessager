using MicroOrm.Dapper.Repositories.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace chatservice.Entities
{
    [Table("Chats")]
    public class Chat
    {
        [Key, Identity]
        public int ChatID { get; set; }

        public virtual Guid ChatUID { get; set; }

        public Guid TenantUID { get; set; }

        public string? Name { get; set; }

        public bool Deleted { get; set; }
    }
}
