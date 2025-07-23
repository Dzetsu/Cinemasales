using Cinemasales.Entities;
using Cinemasales.Entities.PayEntities;
using Cinemasales.Repositories.ResultRepositories;
using Dapper;
using Npgsql;

namespace Cinemasales.Repositories.PayRepositories;

public class PayRepository(NpgsqlDataSource dataSource) : IPayRepository
{
    public async Task Create(PayResult result)
    {
        await using var connection = await dataSource.OpenConnectionAsync();
        const string insertResult = """
                                    INSERT INTO cinemasales.orders_result_list (token, pay_status, username, cost, pincode)
                                    VALUES (@token, @pay_status, @username, @cost, @pincode)
                                    ON CONFLICT (token)
                                        DO UPDATE SET
                                                      pay_status = EXCLUDED.pay_status,
                                                      username = EXCLUDED.username,
                                                      cost = EXCLUDED.cost,
                                                      pincode = EXCLUDED.pincode;
                                    """;
        await connection.ExecuteAsync(insertResult, new {token = result.Token, pay_status = result.Answer, username = result.Username, cost = result.TicketCost, pincode = result.Pincode});
    }
}