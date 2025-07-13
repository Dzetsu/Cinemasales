using Cinemasales.Entities;
using Cinemasales.Repositories;
using Cinemasales.Repositories.Interfaces;
using Cinemasales.Services.Kafka;
using Dapper;
using Npgsql;

namespace Cinemasales.Services.BackgroundServices;

public class MainCoordinator(IMainResultRepository repository) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var resultsList = await repository.GetResults();

            if (resultsList.Count == 0)
            {
                await Task.Delay(5000, cancellationToken);
                continue;
            }

            foreach (var result in resultsList)
            {
                if (result.ResultStatus == 1)
                {
                    await repository.UpdateStatus(result.Token, 1);
                }
                else switch (result)
                {
                    case { PayStatus: 1, ResultStatus: 2 }:
                        var payRollbackMessage = new PayMessage()
                            {  Username = result.Username, Pincode = result.Pincode, Token = result.Token, Cost = result.Cost, Answer = result.ResultStatus};
                        await KafkaProducer.SendMessage(payRollbackMessage, "walletPayRollBack");
                        await repository.UpdateStatus(result.Token, 2);
                        break;
                    case { PayStatus: 2, ResultStatus: 1 }:
                        
                        var bookRollbackMessage = new BookMessage()
                        { SeatNumber = result.SeatNumber, Token = result.Token, Answer = 0 };
                        await KafkaProducer.SendMessage(bookRollbackMessage, "BookSeatRollBack");
                        await repository.UpdateStatus(result.Token, 2);
                        break;
                    default:
                        await repository.UpdateStatus(result.Token, 2);
                        break;
                }
            }
        }
    }
}