using Cinemasales.Entities;
using Cinemasales.Entities.BookEntities;

namespace Cinemasales.Repositories.BookRepository;

public interface IBookRepository
{
    Task Create(BookResult result);
}