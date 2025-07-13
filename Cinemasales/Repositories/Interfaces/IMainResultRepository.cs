using Cinemasales.Entities;

namespace Cinemasales.Repositories.Interfaces;

public interface IMainResultRepository
{
    Task<List<Result>> GetResults();
    Task UpdateStatus(string token, short status);
}