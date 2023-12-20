using System.Text.Json;
using KanbanBoard.Core.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

//
// EXAMPLE OF USING MESSAGE BROKER PUBLISHER
//
namespace KanbanBoard.EventPublisher
{
    public class RabbitMqPublisher
    {
        private readonly string _rabbitMqConnectionString;

        private readonly string _environmentPrefix;

        public RabbitMqPublisher(IOptions<EventBusSettings> settings)
        {
            _rabbitMqConnectionString = settings.Value.HostAddress;
            _environmentPrefix = settings.Value.EnvironmentPrefix;
        }
        
        public void PublishSomeEvent(object data)
        {
            SendMessage("exchange-service:exchange-action", data);
        }

        private void SendMessage(string exchangeName, object contract)
        {
            using (IConnection connection = GetConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(GetExchangeName(exchangeName), type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);
                    channel.BasicPublish(GetExchangeName(exchangeName), routingKey: "", mandatory: false, basicProperties: null, body: JsonSerializer.SerializeToUtf8Bytes(contract));
                }
            }
        }

        private IConnection GetConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_rabbitMqConnectionString)
            };

            return connectionFactory.CreateConnection();
        }

        private string GetExchangeName(string name)
        {
            return $"{_environmentPrefix}:{name}";
        }
    }
}