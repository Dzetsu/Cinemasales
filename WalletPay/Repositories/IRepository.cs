namespace WalletPay.Repositories;

public interface IRepository<T> where T : class
{
    Task UpdateBalance(T entity);
    Task<int> GetBalance(T entity);
}