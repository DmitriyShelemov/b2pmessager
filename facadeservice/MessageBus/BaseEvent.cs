using facadeservice.MessageBus.Interfaces;

namespace facadeservice.MessageBus
{
    public class BaseEvent<T> : IBaseEvent<T> where T : struct, Enum
    {
        public T EventType { get; set; }
    }
}
