using BookSeat.Entities;

namespace BookSeat.Services;

public interface IBookedService
{
    public Task MakeTicketBooked(BookInfo query);
    public Task MakeTicketUnBooked(BookInfo query);
}