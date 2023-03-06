using queuemessagelibrary.MessageBus.Interfaces;

namespace chatservice.Dto
{
    public class ChatCreateDto : ChatDto, IBaseEvent<CrudActionType>, ITenantContext
    {
        public CrudActionType EventType { get; set; }
    }
}
