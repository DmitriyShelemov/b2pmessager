using MicroOrm.Dapper.Repositories.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace messageservice.Entities
{
    [Table("Messages")]
    public class Message
    {
        [Key, Identity]
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
