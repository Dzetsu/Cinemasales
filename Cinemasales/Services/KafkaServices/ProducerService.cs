using System.Text.Json;
using Cinemasales.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Cinemasales.Services.KafkaServices;

public class ProducerService(IOptions<ProducerKafkaSettings> options)
{
    private readonly ProducerKafkaSettings _producerConfig = options.Value ?? throw new ArgumentNullException(nameof(options));

    public async Task SendMessage<T>(T value, string topic)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _producerConfig.BootstrapServers
        };
        
        using var producer = new ProducerBuilder<Null, string>(config).Build();
        var message = JsonSerializer.Serialize(value);
        
        await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
    }
}