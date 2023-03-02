using RabbitMQ.Client;

namespace facadeservice.MessageBus.Interfaces
{
    public interface IMessageConnection
    {
        IConnection GetConnection();
    }
}