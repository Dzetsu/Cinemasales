using System.Diagnostics;
using System.Text.Json;
using Cinemasales.Entities;
using Cinemasales.Repositories.Interfaces;
using Confluent.Kafka;

namespace Cinemasales.Services.BackgroundServices;

public class BookCoordinator(IResultRepository<BookMessage> repository) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var bootstrapServers = "localhost:9092";
        
        var configBook = new ConsumerConfig
        {
            GroupId = "BookConsumerTEST",
            BootstrapServers = bootstrapServers,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        using var bookConsumer = new ConsumerBuilder<Ignore, string>(configBook).Build();
        bookConsumer.Subscribe("bookResult");

        try
        {
            while (true)
            {
                var bookResult = bookConsumer.Consume();
                if (bookResult == null)
                    throw new Exception("bookResult is null");

                var messageBookResult = JsonSerializer.Deserialize<BookMessage>(bookResult.Message.Value);
                if (messageBookResult == null)
                    throw new Exception("messagePayResult or messageBookResult is null");

                await repository.PostResult(messageBookResult);
                bookConsumer.Commit(bookResult);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
}