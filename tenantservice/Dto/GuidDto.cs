using queuemessagelibrary.MessageBus;

namespace tenantservice.Dto
{
    public class GuidDto : BaseEvent<CrudActionType>
    {
        public Guid Id { get; set; }
    }
}
