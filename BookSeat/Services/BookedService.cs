using BookSeat.Entities;
using BookSeat.Repositories;
using BookSeat.Services.Kafka;

namespace BookSeat.Services;

public class BookedService(IBookedRepository repository, BookedProducer producer) : IBookedService
{
    public async Task MakeTicketBooked(BookInfo query)
    {
        var answer = await repository.Update(query);

        query.Answer = answer ? 1 : 0;

        await producer.SendMessage(query);
    }

    public async Task MakeTicketUnBooked(BookInfo query)
    {
        await repository.Update(query);
    }
}