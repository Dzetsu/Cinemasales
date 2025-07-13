using Dapper;
using Npgsql;
using WalletPay.Entities;

namespace WalletPay.Repositories;

public class PaymentRepository(NpgsqlDataSource dataSource) : IPaymentRepository
{
    public async Task<bool> Update(PayInfo userInfo)
    {
        var connection = await dataSource.OpenConnectionAsync();
        
        const string selectIdQuery = """
                                select id from wallet.users_wallets 
                                where username = @username and pincode = @pincode
                                """;
        const string updateWallet = """
                                    update wallet.users_wallets 
                                    set wallet = wallet - @cost 
                                    where id = @id
                                    """;

        var userId = await connection.QuerySingleOrDefaultAsync<int>(selectIdQuery, new { username = userInfo.Username, pincode = userInfo.Pincode, cost = userInfo.Cost });

        if (userId <= 0) return false;
        await connection.ExecuteAsync(updateWallet, new { id = userId });
        return true;
    }
}