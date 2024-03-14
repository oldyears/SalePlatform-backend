using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
namespace Payment.message
{
    public class RabbitMQEventSender
    {
        private readonly ConnectionFactory _factory;
        private readonly string _queueName;

        public RabbitMQEventSender(string queueName)
        {
            _factory = new ConnectionFactory()
            {
                HostName = "106.15.203.185",
                UserName = "oldyear",
                Password = "123456",
                Port = 5672,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(3000) 
            };
            _queueName = queueName;
        }

        public void SendEvent(object eventData, string key)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "payment_event_exchange", type: ExchangeType.Topic);
                    channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    Console.WriteLine("queue declared");
                channel.QueueBind(queue: _queueName, exchange: "payment_event_exchange",routingKey: key);
                var message = JsonConvert.SerializeObject(eventData);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "payment_event_exchange", routingKey: key, basicProperties: null, body: body);
            }
        }
    }
}
