using WalletPay.Entities;

namespace WalletPay.Services.PaymentServices;

public interface IPaymentService
{
    public Task PayTicket(PayQuery query);
    public Task RefundTicket(PayQuery query);
}