using Cinemasales.Entities;

namespace Cinemasales.Services;

public interface ITicketService
{
    Task<IEnumerable<Seat>> GetAllSeats();
    Task CreateOrder(string seatNumber, string username, string pincode);
}