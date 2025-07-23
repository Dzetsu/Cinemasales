using Cinemasales.Entities;
using Dapper;
using Npgsql;

namespace Cinemasales.Repositories.TicketRepositories;

public class TicketRepository(NpgsqlDataSource dataSource) : ITicketRepository
{
    public async Task<IEnumerable<Seat>> GetAll()
    {
        var connection = await dataSource.OpenConnectionAsync();
        
        const string selectSeatsQuery = "SELECT number, cost FROM cinemasales.seats_list";
        
        return await connection.QueryAsync<Seat>(selectSeatsQuery);
    }

    public async Task Create(Order order)
    {
        var connection = await dataSource.OpenConnectionAsync();
        
        const string addOrder = "INSERT INTO cinemasales.orders_list (seat_number, username, token) VALUES (@seat_number, @username, @token)";
        
        await connection.ExecuteAsync(addOrder, new { seat_number = order.SeatNumber, username = order.Username, token = order.Token });
    }
}