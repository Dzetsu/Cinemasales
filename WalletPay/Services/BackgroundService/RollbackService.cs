using System.Diagnostics;
using System.Text.Json;
using Confluent.Kafka;
using WalletPay.Entities;
using WalletPay.Services.Kafka;

namespace WalletPay.Services.BackgroundService;

public class RollbackService(IPaymentService service) : Microsoft.Extensions.Hosting.BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var bootstrapServers = "localhost:9092";
        
        var configForRollBack = new ConsumerConfig
        {
            GroupId = "PayWalletRollBack",
            BootstrapServers = bootstrapServers,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        using var consumerRollBack = new ConsumerBuilder<Ignore, string>(configForRollBack).Build();
        consumerRollBack.Subscribe("walletPayRollBack");

        try
        {
            while (true)
            {
                var consumeResult = consumerRollBack.Consume(cancellationToken);
                
                var message = JsonSerializer.Deserialize<PayInfo>(consumeResult.Message.Value);
                if (message == null)
                    throw new NullReferenceException("Number is null");
                
                await service.MakeTicketRefund(message);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}