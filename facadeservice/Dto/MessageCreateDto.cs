using queuemessagelibrary.MessageBus.Interfaces;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace facadeservice.Dto
{
    public class MessageCreateDto : MessageDto, IBaseEvent<CrudActionType>
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public override Guid MessageUID { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [DefaultValue(CrudActionType.None)]
        public CrudActionType EventType { get; set; }
    }
}
