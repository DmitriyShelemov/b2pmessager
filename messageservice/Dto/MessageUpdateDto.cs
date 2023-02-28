using System.Text.Json.Serialization;

namespace messageservice.Dto
{
    public class MessageUpdateDto : MessageDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public override Guid MessageUID { get; set; }

        [JsonIgnore]
        public override Guid ChatUID { get; set; }
    }
}
