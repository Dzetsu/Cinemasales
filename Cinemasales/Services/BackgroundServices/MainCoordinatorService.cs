using Cinemasales.Entities;
using Cinemasales.Entities.BookEntities;
using Cinemasales.Entities.PayEntities;
using Cinemasales.Enums;
using Cinemasales.Repositories.ResultRepositories;
using Cinemasales.Services.KafkaServices;
using Cinemasales.Settings;
using Microsoft.Extensions.Options;
using static Cinemasales.Enums.ResultStatus;

namespace Cinemasales.Services.BackgroundServices;

public class MainCoordinatorService(IResultRepository repository, ProducerService producer, IOptions<ProducerKafkaSettings> options) : BackgroundService
{
    private readonly ProducerKafkaSettings _producerConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var resultsList = await repository.GetResults();

            if (resultsList.Count == 0)
            {
                await Task.Delay(10000, cancellationToken);
                continue;
            }
            
            foreach (var result in resultsList)
            {
                if (result.ResultStatus == (int)Confirmed)
                {
                    await repository.UpdateStatus(result.Token, (int)Confirmed);
                    continue;
                }
                switch (result)
                {
                    case { PayStatus: (int)Confirmed, ResultStatus: (int)Cancelled }:
                        var payRollbackMessage = new PayMessage
                            {  Username = result.Username, Pincode = result.Pincode, Token = result.Token, Cost = result.Cost};
                        await producer.SendMessage(payRollbackMessage, _producerConfig.PayRollbackTopic);
                        break;
                    
                    case { PayStatus: (int)Confirmed, ResultStatus: (int)Confirmed }:
                        var bookRollbackMessage = new BookMessage 
                            { SeatNumber = result.SeatNumber, Token = result.Token };
                        await producer.SendMessage(bookRollbackMessage, _producerConfig.BookRollbackTopic);
                        break;
                }
                
                await repository.UpdateStatus(result.Token, (int)Cancelled);
                await Task.Delay(5000, cancellationToken);
            }
        }
    }
}