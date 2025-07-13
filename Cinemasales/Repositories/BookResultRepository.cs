using Cinemasales.Entities;
using Cinemasales.Repositories.Interfaces;
using Dapper;
using Npgsql;

namespace Cinemasales.Repositories;

public class BookResultRepository(NpgsqlDataSource dataSource) : IResultRepository<BookMessage>
{
    public async Task PostResult(BookMessage result)
    {
        var connection = await dataSource.OpenConnectionAsync();
        string insertResult = """
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