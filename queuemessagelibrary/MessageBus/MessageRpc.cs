using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using queuemessagelibrary.MessageBus.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace queuemessagelibrary.MessageBus
{
    public class MessageRpc<T> : BackgroundService where T : struct, Enum
    {
        private readonly IMessageConnection _connection;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string _queueName;
        private readonly IModel _channel;

        public MessageRpc(IMessageConnection connection, IServiceScopeFactory scopeFactory, string queueName)
        {
            _connection = connection;
            _scopeFactory = scopeFactory;
            _queueName = queueName;
            try
            {
                _channel = _connection.GetConnection().CreateModel();

                _channel.QueueDeclare(
                    queue: _queueName,
                     durable: true,
                       exclusive: false,
                     autoDelete: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (ModuleHandle, ea) =>
            {
                string? response = string.Empty;

                var body = ea.Body.ToArray();
                var props = ea.BasicProperties;
                var replyProps = _channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    var eventType = JsonSerializer.Deserialize<BaseEvent<T>>(message);

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var processor = scope.ServiceProvider.GetRequiredService<IEventProcessor<BaseEvent<T>>>();
                        response = await processor.ProcessEvent(eventType, message);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($" [.] {e.Message}");
                    response = string.Empty;
                }
                finally
                {
                    var responseBytes = response == null ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(response);
                    _channel.BasicPublish(exchange: string.Empty,
                                         routingKey: props.ReplyTo,
                                         basicProperties: replyProps,
                                         body: responseBytes);
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
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