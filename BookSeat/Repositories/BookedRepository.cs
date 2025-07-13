using BookSeat.Entities;
using Dapper;
using Npgsql;

namespace BookSeat.Repositories;

public class BookedRepository(NpgsqlDataSource dataSource) : IBookedRepository
{
    public async Task<bool> Update(BookInfo query)
    {
        var connection = await dataSource.OpenConnectionAsync();
        
        const string selectIdQuery = """
                                     SELECT id, status 
                                     FROM booked_seats.all_seats
                                     WHERE number = @number 
                                     """;
        const string updateSeatStatus = """
                                    UPDATE booked_seats.all_seats
                                    SET status = @status 
                                    WHERE id = @id
                                    """;

        SeatInfo seat = (await connection.QuerySingleOrDefaultAsync<SeatInfo>(selectIdQuery, new { number = query.SeatNumber }))!;

        switch (seat.Status)
        {
            case 0 when query.Answer == 1:
                return false;
            case 0 when query.Answer == 0:
                await connection.ExecuteAsync(updateSeatStatus, new { id = seat.Id, status = 1 });
                break;
            case 1 when query.Answer == 1:
                await connection.ExecuteAsync(updateSeatStatus, new { id = seat.Id, status = 0 });
                break;
        }
        return true;
    }
}

class SeatInfo
{
    public int Id { get; set; }
    public short Status { get; set; }
}