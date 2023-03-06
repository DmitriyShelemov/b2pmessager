using System.ComponentModel.DataAnnotations.Schema;

namespace messageservice.Entities
{
    [Table("Chats")]
    public class Chat
    {
        public Guid ChatUID { get; set; }

        public Guid TenantUID { get; set; }

        public bool Deleted { get; set; }
    }
}
