using System.Text.Json.Serialization;

namespace MessageService.WebApi.Dto
{
    public class MessageCreateDto : MessageDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public override Guid MessageUID { get; set; }
    }
}
