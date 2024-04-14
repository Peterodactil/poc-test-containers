using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Testcontainers.RabbitMq;
using Testes.Fixture;

namespace Testes.Tests
{
    [Collection("API")]
    public class RabbitMqTest(CustomWebApplicationFactory factory)
    {
        private readonly HttpClient _client = factory.HttpClient;
        private readonly RabbitMqContainer _rabbitMqContainer = RabbitMqConfigurationProvider.Container;

        [Fact]
        public void ConsumeMessageQueue()
        {
            const string queue = "hello";

            const string message = "Hello World!";

            string actualMessage = string.Empty;

            // Signal the completion of message reception.
            EventWaitHandle waitHandle = new ManualResetEvent(false);

            // Create and establish a connection.
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_rabbitMqContainer.GetConnectionString())
            };
            using var connection = connectionFactory.CreateConnection();

            // Send a message to the channel.
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue, false, false, false, null);
            channel.BasicPublish(string.Empty, queue, null, Encoding.Default.GetBytes(message));

            // Consume a message from the channel.
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (_, eventArgs) =>
            {
                actualMessage = Encoding.Default.GetString(eventArgs.Body.ToArray());
                waitHandle.Set();
            };

            channel.BasicConsume(queue, true, consumer);
            waitHandle.WaitOne(TimeSpan.FromSeconds(1));

            Assert.Equal(message, actualMessage);
        }
    }
}
