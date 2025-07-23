using Cinemasales.Entities;

namespace Cinemasales.Services.TicketServices;

public interface ITicketService
{
    Task<IEnumerable<Seat>> GetAllSeats();
    Task CreateOrder(string seatNumber, string username, string pincode);
}