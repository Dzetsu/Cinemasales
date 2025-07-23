using System.Diagnostics;
using System.Text.Json;
using BookSeat.Entities;
using BookSeat.Services.BookedServices;
using Cinemasales.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ConsumerKafkaSettings = BookSeat.Settings.ConsumerKafkaSettings;

namespace BookSeat.Services.BackgroundServices;

public class ResponseForwarder(IBookedService service, IOptions<ConsumerKafkaSettings> options) : BackgroundService
{
    private readonly ConsumerKafkaSettings _consumerSettings = options.Value ?? throw new ArgumentNullException(nameof(options));
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        //Вопрос к Григорию. У меня не получается конфиг поместить за пределами метода, как это можно сделать?
        ConsumerConfig config = new()
        {
            GroupId = _consumerSettings.BookGroup,
            BootstrapServers = _consumerSettings.BootstrapServers,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        using var consumerMain = new ConsumerBuilder<Ignore, string>(config).Build();
        consumerMain.Subscribe(_consumerSettings.BookTopic);

        try
        {
            while (true)
            {
                var consumeResult = consumerMain.Consume(cancellationToken);
                if (consumeResult == null)
                    continue;
                
                var message = JsonSerializer.Deserialize<BookQuery>(consumeResult.Message.Value);
                if (message == null)
                    throw new NullReferenceException("Number is null");
                
                await service.BookTicket(message);
                consumerMain.Commit(consumeResult);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
}