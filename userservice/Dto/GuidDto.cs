using queuemessagelibrary.MessageBus;

namespace userservice.Dto
{
    public class GuidDto : BaseEvent<CrudActionType>
    {
        public Guid Id { get; set; }
    }
}
