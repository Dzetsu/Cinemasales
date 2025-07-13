using System.Diagnostics;
using System.Text.Json;
using BookSeat.Entities;
using Confluent.Kafka;

namespace BookSeat.Services.BackgroundService;

public class RollbackService(IBookedService service) : Microsoft.Extensions.Hosting.BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var bootstrapServers = "localhost:9092";

        var configForRollBack = new ConsumerConfig
        {
            GroupId = "BookRollBack",
            BootstrapServers = bootstrapServers,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumerRollBack = new ConsumerBuilder<Ignore, string>(configForRollBack).Build();
        consumerRollBack.Subscribe("bookSeatRollBack");

        try
        {
            while (true)
            {
                var consumeResult = consumerRollBack.Consume(cancellationToken);

                var message = JsonSerializer.Deserialize<BookInfo>(consumeResult.Message.Value);
                if (message == null)
                    throw new NullReferenceException("Number is null");

                await service.MakeTicketUnBooked(message);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}