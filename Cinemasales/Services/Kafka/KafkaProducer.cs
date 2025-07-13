using System.Text.Json;
using Confluent.Kafka;

namespace Cinemasales.Services.Kafka;

public class KafkaProducer
{
    public static async Task SendMessage<T>(T value, string topic)
    {
        var bootstrapServers = "localhost:9092";

        var config = new ProducerConfig()
        {
            BootstrapServers = bootstrapServers
        };
        
        using var producer = new ProducerBuilder<Null, string>(config).Build();
        string message = JsonSerializer.Serialize(value);
        
        await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
    }
}