using System.Diagnostics;
using System.Text.Json;
using Confluent.Kafka;
using WalletPay.Entities;

namespace WalletPay.Services.BackgroundService;

public class ResponseForwarder(IPaymentService service) : Microsoft.Extensions.Hosting.BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var bootstrapServers = "localhost:9092";

        var config = new ConsumerConfig
        {
            GroupId = "PayWalletTest",
            BootstrapServers = bootstrapServers,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Latest
        };
        
        using var consumerMain = new ConsumerBuilder<Ignore, string>(config).Build();
        consumerMain.Subscribe("PayForSeat");

        try
        {
            while (true)
            {
                var consumeResult = consumerMain.Consume(cancellationToken);
                
                var message = JsonSerializer.Deserialize<PayInfo>(consumeResult.Message.Value);
                if (message == null)
                    throw new NullReferenceException("Number is null");

                await service.MakeTicketPayment(message);
            }

        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
}