using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WorkerService.Models;

namespace WorkerService
{    
    public class Worker : BackgroundService
    {

        //private readonly IInventoryRepository _repo;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private string queueName = "quickorder.received";
        private EventingBasicConsumer _consumer;

        public Worker(IConfiguration config)
        {
            var factory = new ConnectionFactory
            {
                HostName = config.GetValue<string>("RabbitMqHost")
            };

            try
            {
                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                for (var i = 0; i < 5; i++)
                {
                    if (_connection != null)
                        continue;
                    Thread.Sleep(3000);
                    try { _connection = factory.CreateConnection(); } catch { }
                }
                if (_connection == null) throw;
            }

            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false,
                arguments: null);

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += ProcessQuickOrderReceived;

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _channel.BasicConsume(queue: queueName, autoAck: true, consumer: _consumer);
            }

            return Task.CompletedTask;
        }

        private void ProcessQuickOrderReceived(object sender, BasicDeliverEventArgs eventArgs)
        {
            var orderInfo = JsonSerializer.Deserialize<QuickOrderReceivedMessage>(eventArgs.Body.ToArray());

            Log.ForContext("OrderReceived", orderInfo, true)
                .Information("Received message from queue for processing.");            
        }
    }
}
