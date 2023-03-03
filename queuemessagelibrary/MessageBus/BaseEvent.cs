using queuemessagelibrary.MessageBus.Interfaces;

namespace queuemessagelibrary.MessageBus
{
    public class BaseEvent<T> : IBaseEvent<T> where T : struct, Enum
    {
        public T EventType { get; set; }
    }
}
