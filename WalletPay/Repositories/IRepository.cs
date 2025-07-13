namespace WalletPay.Repositories;

public interface IRepository<T> where T : class
{
    Task<bool> Update(T entity);
}