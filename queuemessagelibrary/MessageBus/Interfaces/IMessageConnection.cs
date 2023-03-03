using RabbitMQ.Client;

namespace queuemessagelibrary.MessageBus.Interfaces
{
    public interface IMessageConnection
    {
        IConnection GetConnection();
    }
}