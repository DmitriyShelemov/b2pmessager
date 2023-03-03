using Microsoft.Extensions.Configuration;
using queuemessagelibrary.MessageBus.Interfaces;
using RabbitMQ.Client;

namespace queuemessagelibrary.MessageBus
{
    public class MessageConnection : IMessageConnection
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;

        public MessageConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to MessageBus");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        public IConnection GetConnection()
        {
            return _connection;
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_connection.IsOpen)
                _connection.Close();
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}