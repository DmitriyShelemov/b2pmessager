using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using queuemessagelibrary.MessageBus.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace queuemessagelibrary.MessageBus
{
    public class MessageSubscriber<TMessage> : BackgroundService where TMessage : class
    {
        private readonly IMessageConnection _connection;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string _exchangeName;
        private readonly string _queueName;
        private readonly IModel? _channel;

        public MessageSubscriber(IMessageConnection connection, IServiceScopeFactory scopeFactory, string exchangeName)
        {
            _connection = connection;
            _scopeFactory = scopeFactory;
            _exchangeName = exchangeName;

            try
            {
                _channel = _connection.GetConnection().CreateModel();

                _channel.ExchangeDeclare(
                    exchange: _exchangeName,
                    type: ExchangeType.Fanout,
                    durable: true,
                    autoDelete: false);
                _queueName = _channel.QueueDeclare().QueueName;

                _channel.QueueBind(queue: _queueName,
                    exchange: _exchangeName,
                    routingKey: "");

                Console.WriteLine("--> Listenting on the Message Bus...");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            if (_channel == null)
                throw new InvalidOperationException();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (ModuleHandle, ea) =>
            {
                var body = ea.Body;
 
                try
                {
                    var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                    var eventType = JsonSerializer.Deserialize<TMessage>(notificationMessage);

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var processor = scope.ServiceProvider.GetRequiredService<IEventProcessor<TMessage>>();
                        await processor.ProcessEvent(eventType, notificationMessage);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($" [.] {e.Message}");
                }

                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
            Console.WriteLine("MessageBus Disposed");
            if (_channel?.IsOpen == true)
            {
                _channel.Close();
            }
        }
    }
}