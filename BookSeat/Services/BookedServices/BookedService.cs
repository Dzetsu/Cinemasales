using BookSeat.Entities;
using BookSeat.Repositories;
using BookSeat.Services.KafkaServices;

namespace BookSeat.Services.BookedServices;

public class BookedService(IBookedRepository repository, BookedProducer producer) : IBookedService
{
    public async Task BookTicket(BookQuery query)
    {
        var bookAnswer = new BookAnswer
        {
            SeatNumber = query.SeatNumber,
            Token = query.Token
        };
        
        var status = await repository.GetSeatStatus(query);

        if (status == 1)
        {
            bookAnswer.Answer = 1;
            await repository.UpdateSeat(query, 0);
        }
        else
            bookAnswer.Answer = 0;
        
        
        await producer.SendMessage(query);
    }

    public async Task UnBookTicket(BookQuery query)
    {
        await repository.UpdateSeat(query, 1);
    }
}