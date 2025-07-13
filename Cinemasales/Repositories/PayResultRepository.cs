using Cinemasales.Entities;
using Cinemasales.Repositories.Interfaces;
using Dapper;
using Npgsql;

namespace Cinemasales.Repositories;

public class PayResultRepository(NpgsqlDataSource dataSource) : IResultRepository<PayMessage>
{
    public async Task PostResult(PayMessage result)
    {
        var connection = await dataSource.OpenConnectionAsync();
        string insertResult = """
                              INSERT INTO cinemasales.orders_result_list (token, pay_status, username, cost, pincode)
                              VALUES (@token, @pay_status, @username, @cost, @pincode)
                              ON CONFLICT (token)
                                  DO UPDATE SET
                                                pay_status = EXCLUDED.pay_status,
                                                username = EXCLUDED.username,
                                                cost = EXCLUDED.cost,
                                                pincode = EXCLUDED.pincode;
                              """;
        await connection.ExecuteAsync(insertResult, new {token = result.Token, pay_status = result.Answer, username = result.Username, cost = result.Cost, pincode = result.Pincode});
    }
}