using System.Diagnostics;
using System.Text.Json;
using BookSeat.Entities;
using BookSeat.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace BookSeat;

public class ResponseForwarder(IBookedService service) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var bootstrapServers = "localhost:9092";

        var config = new ConsumerConfig
        {
            GroupId = "BookSeatTest",
            BootstrapServers = bootstrapServers,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Latest
        };
        
        using var consumerMain = new ConsumerBuilder<Ignore, string>(config).Build();
        consumerMain.Subscribe("BookSeat");

        try
        {
            while (true)
            {
                var consumeResult = consumerMain.Consume(cancellationToken);
                
                var message = JsonSerializer.Deserialize<BookInfo>(consumeResult.Message.Value);
                if (message == null)
                    throw new NullReferenceException("Number is null");

                message.Answer = 1;
                await service.MakeTicketBooked(message);
            }

        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
}

/*var bootstrapServers = "localhost:9092";
        var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";
        var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        
        var config = new ConsumerConfig
        {
            GroupId = "BookSeatTEST",
            BootstrapServers = bootstrapServers,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Latest
        };
        
        var configForRollBack = new ConsumerConfig
        {
            GroupId = "BookSeatRollBack",
            BootstrapServers = bootstrapServers,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        BookSeatProducerKafka proudcerKafka = new BookSeatProducerKafka();
        using var consumerMain = new ConsumerBuilder<Ignore, string>(config).Build();
        consumerMain.Subscribe("BookSeat");
        using var consumerRollBack = new ConsumerBuilder<Ignore, string>(configForRollBack).Build();
        consumerRollBack.Subscribe("bookSeatRollBack");

        const string checkStatus = "select id from bookseat.allseats where number = @number and status = 'y'";
        const string updateStatus = "update bookseat.allseats set status = 'n' where id = @id";
        const string updateRollBack = "update bookseat.allseats set status = 'y' where number = @number";

        while (true)
        {
            try
            {
                var consumeResult = consumerMain.Consume(new TimeSpan(0, 0, 10));

                if (consumeResult == null)
                    throw new TimeoutException("Timed out waiting for consumeResult");

                Console.WriteLine("Получил");
                
                var number = JsonSerializer.Deserialize<string>(consumeResult.Message.Value);

                if (number == null)
                    throw new NullReferenceException("Number is null");
                
                Console.WriteLine("Number не null");
                
                var transaction = await connection.BeginTransactionAsync();
                var id = await connection.QueryFirstOrDefaultAsync<int>(checkStatus, new {number = number}, transaction);

                Console.WriteLine("Получил id + Начал транзакцию");
                
                if (id == 0)
                {
                    Console.WriteLine("Ролл бэк");
                    await transaction.RollbackAsync();
                    await proudcerKafka.SendKafkaMessage(number, 'n');
                    Console.WriteLine("Отправил соо N");
                    throw new NullReferenceException("Id is null");
                }
                await connection.ExecuteAsync(updateStatus, new {id = id}, transaction);
                await proudcerKafka.SendKafkaMessage(number, 'y');
                await transaction.CommitAsync();
                
                var consumeResultRollBack = consumerRollBack.Consume(new TimeSpan(0, 0, 15));
                
                if (consumeResultRollBack == null)
                    throw new TimeoutException("Timed out waiting for consumeResultRollBack");
                
                var numbeRollBack = JsonSerializer.Deserialize<string>(consumeResultRollBack.Message.Value);
                
                if (numbeRollBack == null)
                    throw new NullReferenceException("MessageRollBack is null");
                
                Console.WriteLine("Отправил 2");
                
                await connection.ExecuteAsync(updateRollBack, new {number = numbeRollBack});
            }
            catch (TimeoutException e)
            {
                Debug.WriteLine(e);
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e);
            }
        }*/