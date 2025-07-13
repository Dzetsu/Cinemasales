namespace Cinemasales.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task Create(T value);
}