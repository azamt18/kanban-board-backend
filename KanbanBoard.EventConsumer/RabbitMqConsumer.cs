using System.Text.Json;
using KanbanBoard.Core.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace KanbanBoard.EventConsumer
{
    /// <summary>
    /// EXAMPLE OF USING MESSAGE BROKER SUBSCRIBER/CONSUMER
    /// </summary>
    public class RabbitMqConsumer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IModel _channel;

        private readonly IConnection _connection;

        private readonly string _rabbitMqHostAddress;

        private readonly string _environmentPrefix;

        public RabbitMqConsumer(IServiceProvider serviceProvider, IOptions<EventBusSettings> settings)
        {
            _rabbitMqHostAddress = settings.Value.HostAddress;
            _environmentPrefix = settings.Value.EnvironmentPrefix;
            _serviceProvider = serviceProvider;

            _connection = GetConnection();
            _channel = _connection.CreateModel();
        }

        private IConnection GetConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_rabbitMqHostAddress)
            };

            return connectionFactory.CreateConnection();
        }

        private string GetExchangeName(string name)
        {
            return $"{_environmentPrefix}:{name}";
        }

        private void RegisterConsumers()
        {
            try
            {
                OnSomethingHappened("exchange-service:exchange-action", "consumer-service:consumer-action");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void OnSomethingHappened(string commonExchangeName, string queue)
        {
            var exchangeName = GetExchangeName(commonExchangeName);

            IModel channel = CreateChannel();
            channel.ExchangeDeclare(exchangeName, type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);
            channel.QueueDeclare(queue, true, false, false, null);
            channel.QueueBind(queue: queue, exchange: exchangeName, routingKey: string.Empty, arguments: null);

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (_, ea) =>
            {
                try
                {
                    using (var ms = new MemoryStream(ea.Body.ToArray()))
                    {
                        var contractor = await JsonSerializer.DeserializeAsync<object>(ms);
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var service = scope.ServiceProvider.GetRequiredService<object>();
                            
                            // you can implement your logic
                            var success = true;
                            if (success)
                                channel.BasicAck(ea.DeliveryTag, false);
                            else
                                channel.BasicNack(ea.DeliveryTag, false, true);
                        }
                    }
                }
                catch (Exception e)
                {
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };
            
            channel.BasicConsume(queue, false, consumer);
        }
        
        private IModel CreateChannel() => _connection.CreateModel();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // when the service is stopping
            // dispose these references
            // to prevent leaks
            if (cancellationToken.IsCancellationRequested)
            {
                _channel.Dispose();
                _connection.Dispose();
                return Task.CompletedTask;
            }

            RegisterConsumers();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // when the service is stopping
            // dispose these references
            // to prevent leaks
            if (cancellationToken.IsCancellationRequested)
            {
                _channel.Dispose();
                _connection.Dispose();
            }

            return Task.CompletedTask;
        }
    }
}