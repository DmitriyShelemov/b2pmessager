using queuemessagelibrary.MessageBus.Interfaces;

namespace messageservice.Dto
{
    public class MessageCreateDto : MessageDto, IBaseEvent<CrudActionType>, ITenantContext
    {
        public CrudActionType EventType { get; set; }
    }
}
