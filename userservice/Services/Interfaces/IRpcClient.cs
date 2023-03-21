using queuemessagelibrary.MessageBus.Interfaces;

namespace userservice.Services.Interfaces
{
    public interface IRpcClient<T> : IMessageRpcClient where T : class
    {
    }
}
