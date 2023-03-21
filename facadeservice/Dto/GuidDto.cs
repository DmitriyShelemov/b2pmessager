using queuemessagelibrary.MessageBus;

namespace facadeservice.Dto
{
    public class GuidDto : BaseEvent<CrudActionType>
    {
        public Guid Id { get; set; }
    }
}
