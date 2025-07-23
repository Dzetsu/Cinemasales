using System.Diagnostics;
using System.Text.Json;
using BookSeat.Entities;
using BookSeat.Services.BookedServices;
using Cinemasales.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using ConsumerKafkaSettings = BookSeat.Settings.ConsumerKafkaSettings;

namespace BookSeat.Services.BackgroundServices;

public class RollbackService(IBookedService service, IOptions<ConsumerKafkaSettings> options) : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly ConsumerKafkaSettings _rollbackConsumerSettings = options.Value ?? throw new ArgumentNullException(nameof(options));
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    { 
        //Вопрос к Григорию. У меня не получается конфиг поместить за пределами метода, как это можно сделать?
        ConsumerConfig rollbackConsumerConfig = new()
        {
            GroupId = _rollbackConsumerSettings.BookRollbackGroup,
            BootstrapServers = _rollbackConsumerSettings.BootstrapServers,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        using var consumerRollBack = new ConsumerBuilder<Ignore, string>(rollbackConsumerConfig).Build();
        consumerRollBack.Subscribe(_rollbackConsumerSettings.BookRollbackTopic);

        try
        {
            while (true)
            {
                var consumeResult = consumerRollBack.Consume(cancellationToken);

                var message = JsonSerializer.Deserialize<BookQuery>(consumeResult.Message.Value);
                if (message == null)
                    throw new NullReferenceException("Number is null");
                
                await service.UnBookTicket(message);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}