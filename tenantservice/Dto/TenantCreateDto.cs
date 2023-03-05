using queuemessagelibrary.MessageBus.Interfaces;

namespace tenantservice.Dto
{
    public class TenantCreateDto : TenantDto, IBaseEvent<CrudActionType>
    {
        public override Guid TenantUID { get; set; }

        public CrudActionType EventType { get; set; }
    }
}
