using queuemessagelibrary.MessageBus.Interfaces;

namespace facadeservice.Services.Interfaces
{
    public interface IRpcClient<T> : IMessageRpcClient where T : class
    {
    }
}
