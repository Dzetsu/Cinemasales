using System.Diagnostics;
using System.Text.Json;
using Cinemasales.Entities;
using Cinemasales.Entities.PayEntities;
using Cinemasales.Repositories;
using Cinemasales.Repositories.PayRepositories;
using Cinemasales.Services.BackgroundServices;
using Cinemasales.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Cinemasales.Services.BackgroundServices;

public class PayCoordinatorService(IPayRepository repository, IOptions<ConsumerKafkaSettings> kafkaOptions) : BackgroundService
{
    
    private readonly ConsumerKafkaSettings _payConsumerSetting = kafkaOptions.Value ?? throw new ArgumentNullException(nameof(kafkaOptions));
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        //Вопрос к Григорию. У меня не получается конфиг поместить за пределами метода, как это можно сделать?
        var configPay = new ConsumerConfig
        {
            GroupId = _payConsumerSetting.PayGroup,
            BootstrapServers = _payConsumerSetting.BootstrapServers,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        using var payConsumer = new ConsumerBuilder<Ignore, string>(configPay).Build();
        payConsumer.Subscribe(_payConsumerSetting.PayTopic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var payResult = payConsumer.Consume(TimeSpan.FromSeconds(1));
                if (payResult == null)
                    throw new Exception("payResult is null");

                var payResultMessage = JsonSerializer.Deserialize<PayResult>(payResult.Message.Value);
                if (payResultMessage == null)
                    throw new Exception("messagePayResult is null");
                
                await repository.Create(payResultMessage);
                payConsumer.Commit(payResult);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
}