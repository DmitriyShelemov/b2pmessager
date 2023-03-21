using MicroOrm.Dapper.Repositories.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace userservice.Entities
{
    [Table("Users")]
    public class User
    {
        [Key, Identity]
        public int UserID { get; set; }

        public virtual Guid UserUID { get; set; }

        public string? Name { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? VerificationKey { get; set; }

        public bool Activated { get; set; }

        public bool Deleted { get; set; }
    }
}
