using Cinemasales.Entities;
using Cinemasales.Enums;
using Dapper;
using Npgsql;

namespace Cinemasales.Repositories.ResultRepositories;

public class ResultRepository(NpgsqlDataSource dataSource) : IResultRepository
{
    public async Task<List<Result>> GetResults()
    {
        await using var connection = await dataSource.OpenConnectionAsync();
        const string selectResultsQuery = """
                                          SELECT * 
                                          FROM cinemasales.orders_result_list 
                                          WHERE result_status IN (1, 2)
                                          LIMIT 50
                                          """;
        return (await connection.QueryAsync<Result>(selectResultsQuery)).ToList();
    }

    public async Task UpdateStatus(string token, int status)
    {
        await using var connection = await dataSource.OpenConnectionAsync();
        const string updateStatus = """
                                    UPDATE cinemasales.orders_list 
                                    SET status = @status
                                    WHERE token = @token
                                    """;
        
        await connection.ExecuteAsync(updateStatus, new { status });
    }
}