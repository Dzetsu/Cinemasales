using System.Text.Json;
using Confluent.Kafka;

namespace BookSeat.Services.Kafka;

public class BookedProducer
{
    public async Task SendMessage<T>(T payMessage)
    {
        var bootstrapServers = "localhost:9092";

        var config = new ProducerConfig()
        {
            BootstrapServers = bootstrapServers
        };
        
        using var producer = new ProducerBuilder<Null, string>(config).Build();
        string message = JsonSerializer.Serialize(payMessage);
        
        await producer.ProduceAsync("bookResult", new Message<Null, string> { Value = message });
    }
}