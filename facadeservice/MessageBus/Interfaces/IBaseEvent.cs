namespace facadeservice.MessageBus.Interfaces
{
    public interface IBaseEvent<T> where T : struct, Enum
    {
        T EventType { get; set; }
    }
}