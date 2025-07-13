namespace Cinemasales.Repositories.Interfaces;

public interface IResultRepository<T> where T : class
{
    public Task PostResult(T result);
}