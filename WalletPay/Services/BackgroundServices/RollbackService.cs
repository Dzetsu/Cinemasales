using System.Diagnostics;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using WalletPay.Entities;
using WalletPay.Services.PaymentServices;
using WalletPay.Settings;

namespace WalletPay.Services.BackgroundServices;

public class RollbackService(IPaymentService service, IOptions<ConsumerKafkaSettings> options) : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly ConsumerKafkaSettings _consumerKafkaSettings = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        ConsumerConfig configForRollBack = new()
        {
            GroupId = _consumerKafkaSettings.PayRollbackGroup,
            BootstrapServers = _consumerKafkaSettings.BootstrapServers,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        using var consumerRollBack = new ConsumerBuilder<Ignore, string>(configForRollBack).Build();
        consumerRollBack.Subscribe(_consumerKafkaSettings.PayRollbackTopic);

        try
        {
            while (true)
            {
                var consumeResult = consumerRollBack.Consume(cancellationToken);
                
                var message = JsonSerializer.Deserialize<PayQuery>(consumeResult.Message.Value);
                if (message == null)
                    throw new NullReferenceException("Number is null");
                
                await service.RefundTicket(message);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}