using queuemessagelibrary.MessageBus.Interfaces;
using System.Text.Json.Serialization;

namespace chatservice.Dto
{
    public class ChatCreateDto : ChatDto, IBaseEvent<CrudActionType>, ITenantContext
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public override Guid ChatUID { get; set; }

        public CrudActionType EventType { get; set; }
    }
}
