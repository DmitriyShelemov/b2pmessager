namespace queuemessagelibrary.MessageBus.Interfaces
{
    public interface IMessagePublisher<T> : IDisposable where T : class
    {
        void Publish(T dto);
    }
}