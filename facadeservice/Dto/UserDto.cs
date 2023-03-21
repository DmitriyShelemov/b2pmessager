using System.Text.Json.Serialization;

namespace facadeservice.Dto
{
    public class UserDto
    {
        public virtual Guid UserUID { get; set; }

        public virtual string? Name { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public virtual string? Email { get; set; }

        public virtual bool Activated { get; set; }
    }
}
