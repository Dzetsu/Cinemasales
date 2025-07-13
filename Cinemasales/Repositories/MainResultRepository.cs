using Cinemasales.Entities;
using Cinemasales.Repositories.Interfaces;
using Dapper;
using Npgsql;

namespace Cinemasales.Repositories;

public class MainResultRepository(NpgsqlDataSource dataSource) : IMainResultRepository
{
    public async Task<List<Result>> GetResults()
    {
        var connection = await dataSource.OpenConnectionAsync();
        string selectResultsQuery = """
                                    SELECT * 
                                    FROM cinemasales.orders_result_list 
                                    WHERE result_status IN (1, 2)
                                    LIMIT 50
                                    """;
        return (await connection.QueryAsync<Result>(selectResultsQuery)).ToList();
    }

    public async Task UpdateStatus(string token, short status)
    {
        var connection = await dataSource.OpenConnectionAsync();
        string updateStatus = """
                              UPDATE cinemasales.orders_list 
                              SET status = @status
                              WHERE token = @token
                              """;
        await connection.ExecuteAsync(updateStatus, new { status = status });
    }
}