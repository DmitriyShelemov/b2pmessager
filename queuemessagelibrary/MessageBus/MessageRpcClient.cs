using queuemessagelibrary.MessageBus.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace queuemessagelibrary.MessageBus
{
    public class MessageRpcClient : IMessageRpcClient
    {
        private readonly IMessageConnection _connection;
        private readonly string _queueName;
        private readonly IModel _channel;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _callbackMapper = new();

        public MessageRpcClient(IMessageConnection connection, string queueName)
        {
            _connection = connection;
            _queueName = queueName;
            try
            {
                _channel = _connection.GetConnection().CreateModel();

                _channel.QueueDeclare(
                    queue: _queueName,
                     durable: true,
                       exclusive: false,
                     autoDelete: false);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (model, ea) =>
                {
                    if (!_callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                        return;
                    var body = ea.Body.ToArray();
                    var response = Encoding.UTF8.GetString(body);
                    tcs.TrySetResult(response);
                };

                _channel.BasicConsume(consumer: consumer,
                                     queue: _queueName,
                                     autoAck: true);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest dto)
            where TRequest : class
            where TResponse : class
        {
            var message = JsonSerializer.Serialize(dto);

            if (_connection.GetConnection().IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
                var response = await CallAsync(message);
                return JsonSerializer.Deserialize<TResponse>(response);
            }

            Console.WriteLine("--> RabbitMQ connectionis closed, not sending");

            return null;
        }

        private Task<string> CallAsync(string message, CancellationToken cancellationToken = default)
        {
            IBasicProperties props = _channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = _queueName;
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var tcs = new TaskCompletionSource<string>();
            _callbackMapper.TryAdd(correlationId, tcs);

            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: _queueName,
                                 basicProperties: props,
                                 body: messageBytes);

            cancellationToken.Register(() => _callbackMapper.TryRemove(correlationId, out _));
            return tcs.Task;
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