using Cinemasales.Entities;
using Cinemasales.Entities.PayEntities;

namespace Cinemasales.Repositories.PayRepositories;

public interface IPayRepository
{
    Task Create(PayResult result);
}