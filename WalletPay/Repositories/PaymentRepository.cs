using Dapper;
using Npgsql;
using WalletPay.Entities;

namespace WalletPay.Repositories;

public class PaymentRepository(NpgsqlDataSource dataSource) : IPaymentRepository
{
    private const string SelectWalletQuery = """
                                         select wallet from wallet.users_wallets 
                                         where username = @username and pincode = @pincode
                                         """;

    private const string UpdateWallet = """
                                        update wallet.users_wallets 
                                        set wallet = wallet - @cost 
                                        where username = @username and pincode = @pincode
                                        """;

    public async Task<int> GetBalance(PayQuery userQuery)
    {
        await using var connection = await dataSource.OpenConnectionAsync();
        var balance = await connection.QueryFirstAsync<int>(SelectWalletQuery, new { username = userQuery.Username, pincode = userQuery.Pincode });
        return balance;
    }
    
    public async Task UpdateBalance(PayQuery userQuery)
    {
        await using var connection = await dataSource.OpenConnectionAsync();
        await connection.ExecuteAsync(UpdateWallet, new { username = userQuery.Username, pincode = userQuery.Pincode, cost = userQuery.Cost });
    }
}