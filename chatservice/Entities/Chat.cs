using MicroOrm.Dapper.Repositories.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace chatservice.Entities
{
    [Table("Chats")]
    public class Chat
    {
        [Key, Identity]
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
