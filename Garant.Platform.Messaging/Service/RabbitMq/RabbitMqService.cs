using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Garant.Platform.Messaging.Abstraction.RabbitMq;
using Garant.Platform.Messaging.Model.Output;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Garant.Platform.Messaging.Service.RabbitMq
{
    /// TODO: на MVP не используем, пока нет необходимости. Возможно будет доработан после MVP при масштабировании.
    /// <summary>
    /// Сервис RabbitMQ.
    /// </summary>
    public sealed class RabbitMqService : BackgroundService, IRabbitMqService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqService(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public async Task SendMessagesRabbitMqAsync(string message)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _configuration.GetSection("RabbitMQ:Host").Value,
                    UserName = _configuration.GetSection("RabbitMQ:Login").Value,
                    Password = _configuration.GetSection("RabbitMQ:Password").Value
                };

                // Создаст подключение.
                using var connection = factory.CreateConnection();

                // Создаст канал.
                using var channel = connection.CreateModel();

                // Подключится к очереди.
                channel.QueueDeclare("Messages", false, false, false, null);
                var body = Encoding.UTF8.GetBytes(message);

                // Добавит контент к очереди.
                channel.BasicPublish("", "Messages", null, body);
                Console.WriteLine("Отправленное сообщение: {0} ", message);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<ReceiveRabbitMqMessageOutput>> ReceiveMessagesRabbitMqAsync()
        {
            try
            {
                return null;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var messagesQueueList = new List<string>();
                var factory = new ConnectionFactory
                {
                    HostName = _configuration.GetSection("RabbitMQ:Host").Value,
                    UserName = _configuration.GetSection("RabbitMQ:Login").Value,
                    Password = _configuration.GetSection("RabbitMQ:Password").Value
                };

                // Создаст подключение.
                using var connection = factory.CreateConnection();

                // Создаст канал.
                using var channel = connection.CreateModel();

                // Подключится к очереди.
                channel.QueueDeclare("Messages", false, false, false, null);

                // create a consumer that listens on the channel (queue)
                var consumer = new EventingBasicConsumer(channel);

                // handle the Received event on the consumer
                // this is triggered whenever a new message
                // is added to the queue by the producer

                consumer.Received += async (model, ea) =>
                {
                    // Чтение из байтов.
                    var body = ea.Body.ToArray();

                    // Приведение сообщения к строке.
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine("Полученное сообщение из очереди: {0}", message);

                    // Send message to all users in SignalR
                    //chatHub.Clients.All.SendAsync("messageReceived", "You have received a message");

                    messagesQueueList.Add(message);
                };

                channel.BasicConsume("Messages", true, consumer);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
