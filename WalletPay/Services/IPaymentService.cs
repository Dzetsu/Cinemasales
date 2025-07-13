using WalletPay.Entities;

namespace WalletPay.Services;

public interface IPaymentService
{
    public Task MakeTicketPayment(PayInfo query);
    public Task MakeTicketRefund(PayInfo query);
}