using System.Text.Json.Serialization;

namespace chatservice.Dto
{
    public class ChatCreateDto : ChatDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public override Guid ChatUID { get; set; }
    }
}
