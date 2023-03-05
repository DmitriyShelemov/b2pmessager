using queuemessagelibrary.MessageBus;

namespace facadeservice.Dto
{
    public class GuidDto : BaseEvent<CrudActionType>, ITenantContext
    {
        public Guid Id { get; set; }

        public Guid TenantUID { get; set; }
    }
}
