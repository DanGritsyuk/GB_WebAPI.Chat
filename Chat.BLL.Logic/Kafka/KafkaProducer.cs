using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Chat.BLL.Logic.Contracts.Kafka;

namespace Chat.BLL.Logic.Kafka
{
    public class KafkaProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>
    {
        private readonly ILogger<KafkaProducer<TKey, TValue>> _logger;
        private readonly ProducerConfig _config;

        public KafkaProducer(ILogger<KafkaProducer<TKey, TValue>> logger, string bootstrapServers) 
        { 
            _logger = logger;
            _config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
            };
        }

        public async Task ProduceAsync(string topic, TValue message)
        {
            try
            {
                using (var producer = new ProducerBuilder<TKey, TValue>(_config).Build())
                {
                    var result = await producer.ProduceAsync(topic, new Message<TKey, TValue> { Value = message });
                    _logger.LogInformation($"Message produced to {result.TopicPartitionOffset}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while producing the message.");
            }
        }
    }
}
