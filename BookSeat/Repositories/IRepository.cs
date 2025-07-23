namespace BookSeat.Repositories;

public interface IRepository<T> where T : class
{
    Task UpdateSeat(T query, short status);
    Task<short> GetSeatStatus(T entity);
}