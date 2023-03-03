using System;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using queuemessagelibrary.MessageBus.Interfaces;
using RabbitMQ.Client;

namespace queuemessagelibrary.MessageBus
{
    public class MessagePublisher<T> : IMessagePublisher<T> where T : class
    {
        private readonly IMessageConnection _connection;
        private readonly string _exchangeName;
        private readonly IModel _channel;

        public MessagePublisher(IMessageConnection connection, string exchangeName)
        {
            _connection = connection;
            _exchangeName = exchangeName;

            try
            {
                _channel = _connection.GetConnection().CreateModel();

                _channel.ExchangeDeclare(
                    exchange: _exchangeName,
                    type: ExchangeType.Fanout,
                    durable: true,
                    autoDelete: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        public void Publish(T dto)
        {
            var message = JsonSerializer.Serialize(dto);
            var conn = _connection.GetConnection();
            if (conn.IsOpen)
            {
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connectionis closed, not sending");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: _exchangeName,
                            routingKey: "",
                            basicProperties: null,
                            body: body);
            Console.WriteLine($"--> We have sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
            }
        }
    }
}