using System.Diagnostics;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using WalletPay.Entities;
using WalletPay.Services.PaymentServices;
using WalletPay.Settings;

namespace WalletPay.Services.BackgroundServices;

public class ResponseForwarderService(IPaymentService service, IOptions<ConsumerKafkaSettings> options) : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly ConsumerKafkaSettings _consumerKafkaSettings = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        ConsumerConfig config = new()
        {
            GroupId = _consumerKafkaSettings.PayGroup,
            BootstrapServers = _consumerKafkaSettings.BootstrapServers,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Latest
        };
        
        using var consumerMain = new ConsumerBuilder<Ignore, string>(config).Build();
        consumerMain.Subscribe(_consumerKafkaSettings.PayTopic);

        try
        {
            while (true)
            {
                var consumeResult = consumerMain.Consume(cancellationToken);
                
                var message = JsonSerializer.Deserialize<PayQuery>(consumeResult.Message.Value);
                if (message == null)
                    throw new NullReferenceException("Number is null");
                
                await service.PayTicket(message);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
}