using Cinemasales.Entities;

namespace Cinemasales.Repositories.TicketRepositories;

public interface ITicketRepository
{
    Task<IEnumerable<Seat>> GetAll();
    Task Create(Order order);
}