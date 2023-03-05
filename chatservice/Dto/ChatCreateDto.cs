using queuemessagelibrary.MessageBus.Interfaces;
using System.Text.Json.Serialization;

namespace chatservice.Dto
{
    public class ChatCreateDto : ChatDto, IBaseEvent<CrudActionType>, ITenantContext
    {
        public override Guid ChatUID { get; set; }

        public CrudActionType EventType { get; set; }
    }
}
