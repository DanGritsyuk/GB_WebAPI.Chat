namespace Chat.BLL.Logic.Contracts.Kafka
{
    public interface IKafkaProducer<TKey, TValue>
    {
        Task ProduceAsync(string topic, TValue message);
    }
}