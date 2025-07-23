using Cinemasales.Entities;
using Cinemasales.Enums;

namespace Cinemasales.Repositories.ResultRepositories;

public interface IResultRepository
{
    Task<List<Result>> GetResults();
    Task UpdateStatus(string token, int status);
}