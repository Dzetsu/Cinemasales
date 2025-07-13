using System.Diagnostics;
using System.Text.Json;
using Cinemasales.Entities;
using Cinemasales.Repositories;
using Cinemasales.Repositories.Interfaces;
using Confluent.Kafka;

namespace Cinemasales.Services.BackgroundServices;

public class PayCoordinator(IResultRepository<PayMessage> repository) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var bootstrapServers = "localhost:9092";

        var configPay = new ConsumerConfig
        {
            GroupId = "PayConsumerTEST",
            BootstrapServers = bootstrapServers,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var payConsumer = new ConsumerBuilder<Ignore, string>(configPay).Build();
        payConsumer.Subscribe("payResult");

        try
        {
            while (true)
            {
                var payResult = payConsumer.Consume(cancellationToken);
                if (payResult == null)
                    throw new Exception("payResult is null");

                var messagePayResult = JsonSerializer.Deserialize<PayMessage>(payResult.Message.Value);
                if (messagePayResult == null)
                    throw new Exception("messagePayResult is null");

                await repository.PostResult(messagePayResult);
                payConsumer.Commit(payResult);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
}