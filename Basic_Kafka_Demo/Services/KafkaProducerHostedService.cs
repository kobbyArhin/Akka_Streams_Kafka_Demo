using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Basic_Kafka_Demo.Services
{
    class KafkaProducerHostedService : IHostedService
    {
        private readonly ILogger<KafkaProducerHostedService> _logger;
        private IProducer<Null, string> _producer;

        public KafkaProducerHostedService(ILogger<KafkaProducerHostedService> logger)
        {
            _logger = logger;
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092"
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            for (var i = 1; i < 20; ++i)
            {
                var value = $"Producer Implementation no {i} of Kafka Streams on topic: demo";
                _logger.LogInformation(value);
                await _producer.ProduceAsync("demo", new Message<Null, string>()
                {
                    Value = value
                }, cancellationToken);
            }
            _producer.Flush(TimeSpan.FromSeconds(10));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _producer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
