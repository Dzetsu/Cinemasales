using BookSeat.Entities;
using Dapper;
using Npgsql;

namespace BookSeat.Repositories;

public class BookedRepository(NpgsqlDataSource dataSource) : IBookedRepository
{
    private const string SelectStatusQuery = """
                                         SELECT status 
                                         FROM booked_seats.all_seats
                                         WHERE number = @number 
                                         """;

    private const string UpdateSeatStatus = """
                                            UPDATE booked_seats.all_seats
                                            SET status = @status 
                                            WHERE number = @number 
                                            """;
    public async Task UpdateSeat(BookQuery query, short status)
    {
        await using var connection = await dataSource.OpenConnectionAsync();
        
        await connection.ExecuteAsync(UpdateSeatStatus, new { number = query.SeatNumber, status });
    }

    public async Task<short> GetSeatStatus(BookQuery query)
    {
        await using var connection = await dataSource.OpenConnectionAsync();
        
        var status = (await connection.QuerySingleOrDefaultAsync<short>(SelectStatusQuery, new { number = query.SeatNumber }))!;

        return status;
    }
}