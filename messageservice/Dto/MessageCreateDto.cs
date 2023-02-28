using System.Text.Json.Serialization;

namespace messageservice.Dto
{
    public class MessageCreateDto : MessageDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public override Guid MessageUID { get; set; }
    }
}
