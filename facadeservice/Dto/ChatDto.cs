using System.Text.Json.Serialization;

namespace facadeservice.Dto
{
    public class ChatDto
    {
        public virtual Guid ChatUID { get; set; }

        public string? Name { get; set; }
    }
}
