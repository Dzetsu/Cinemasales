using Cinemasales.Entities;

namespace Cinemasales.Repositories.Interfaces;

public interface ITicketRepository : IRepository<Order>
{
    Task<IEnumerable<Seat>> GetAll();
}