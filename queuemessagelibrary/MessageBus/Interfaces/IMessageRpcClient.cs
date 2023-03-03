namespace queuemessagelibrary.MessageBus.Interfaces
{
    public interface IMessageRpcClient : IDisposable
    {
        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest dto)
            where TRequest : class
            where TResponse : class;
    }
}