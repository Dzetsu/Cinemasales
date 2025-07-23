using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using WalletPay.Settings;

namespace WalletPay.Services.KafkaServices;

public class PaymentProducer(IOptions<ProducerKafkaSettings> options)
{
    private readonly ProducerKafkaSettings _producerSettings = options.Value ?? throw new ArgumentNullException(nameof(options));
    
    public async Task SendMessage<T>(T payMessage)
    {
        var config = new ProducerConfig()
        {
            BootstrapServers = _producerSettings.BootstrapServers
        };
        
        using var producer = new ProducerBuilder<Null, string>(config).Build();
        var message = JsonSerializer.Serialize(payMessage);
        
        await producer.ProduceAsync(_producerSettings.PayTopic, new Message<Null, string> { Value = message });
    }
}