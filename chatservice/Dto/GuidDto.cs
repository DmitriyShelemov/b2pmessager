using queuemessagelibrary.MessageBus;

namespace chatservice.Dto
{
    public class GuidDto : BaseEvent<CrudActionType>, ITenantContext
    {
        public Guid Id { get; set; }

        public Guid TenantUID { get; set; }
    }
}
