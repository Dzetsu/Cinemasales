using System.Diagnostics;
using System.Text.Json;
using Cinemasales.Entities;
using Cinemasales.Entities.BookEntities;
using Cinemasales.Repositories.BookRepository;
using Cinemasales.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Cinemasales.Services.BackgroundServices;

public class BookCoordinatorService(IBookRepository repository, IOptions<ConsumerKafkaSettings> kafkaOptions) : BackgroundService
{
    private readonly ConsumerKafkaSettings _bookConsumerSetting = kafkaOptions.Value ?? throw new ArgumentNullException(nameof(kafkaOptions));
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        //Вопрос к Григорию. У меня не получается конфиг поместить за пределами метода, как это можно сделать?
        ConsumerConfig configBook = new()
        {
            GroupId = _bookConsumerSetting.BookGroup,
            BootstrapServers = _bookConsumerSetting.BootstrapServers,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        using var bookConsumer = new ConsumerBuilder<Ignore, string>(configBook).Build();
        bookConsumer.Subscribe(_bookConsumerSetting.BookTopic);

        try
        {
            while (true)
            {
                var bookResult = bookConsumer.Consume(cancellationToken);
                if (bookResult == null)
                    throw new Exception("bookResult is null");

                var messageBookResult = JsonSerializer.Deserialize<BookResult>(bookResult.Message.Value);
                if (messageBookResult == null)
                    throw new Exception("messageBookResult is null");
                
                await repository.Create(messageBookResult);
                bookConsumer.Commit(bookResult);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
}