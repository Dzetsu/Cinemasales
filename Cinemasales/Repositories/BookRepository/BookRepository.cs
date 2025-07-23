using Cinemasales.Entities;
using Cinemasales.Entities.BookEntities;
using Dapper;
using Npgsql;

namespace Cinemasales.Repositories.BookRepository;

public class BookRepository(NpgsqlDataSource dataSource) : IBookRepository
{
    public async Task Create(BookResult result)
    {
        await using var connection = await dataSource.OpenConnectionAsync();
        const string insertResult = """
                                    INSERT INTO cinemasales.orders_result_list (token, book_status, seat_number)
                                    VALUES (@token, @book_status, @seat_number)
                                    ON CONFLICT (token)
                                        DO UPDATE SET
                                                      book_status = EXCLUDED.book_status,
                                                      seat_number = EXCLUDED.seat_number;
                                    """;
        await connection.ExecuteAsync(insertResult, new {token = result.Token, book_status = result.Answer, seat_number = result.SeatNumber});
    }
}