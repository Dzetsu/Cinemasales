namespace BookSeat.Repositories;

public interface IRepository<T> where T : class
{
    Task<bool> Update(T query);
}