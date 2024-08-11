using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Chat.BLL.Logic.Contracts.Kafka;
using Chat.BLL.Logic.Contracts.Notififcation;
using EmailService.Common.Contracts;

namespace Chat.BLL.Logic.Notification
{
    public class NotificationLogic : INotificationLogic
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IKafkaProducer<Ignore, EmailServiceMessage> _kafkaProducer;
        private readonly ILogger<NotificationLogic> _logger;

        public NotificationLogic(
            IHttpClientFactory httpClientFactory,
            IKafkaProducer<Ignore, EmailServiceMessage> kafkaProducer,
            ILogger<NotificationLogic> logger)
        {
            _httpClientFactory = httpClientFactory;
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        public async Task SendAsync(EmailServiceMessage message)
        {
            try
            {
                await _kafkaProducer.ProduceAsync("email.service.topic", message);
                _logger.LogInformation("message was sended sucsessful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
