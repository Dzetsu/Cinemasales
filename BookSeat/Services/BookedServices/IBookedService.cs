using BookSeat.Entities;

namespace BookSeat.Services.BookedServices;

public interface IBookedService
{
    public Task BookTicket(BookQuery query);
    public Task UnBookTicket(BookQuery query);
}