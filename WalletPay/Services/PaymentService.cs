using WalletPay.Entities;
using WalletPay.Repositories;
using WalletPay.Services.Kafka;

namespace WalletPay.Services;

public class PaymentService(IPaymentRepository repository, PaymentProducer producer) : IPaymentService
{
    public async Task MakeTicketPayment(PayInfo query)
    {
        var answer = await repository.Update(query);

        if (answer)
        {
            query.Answer = 1;
            await producer.SendMessage(query);
        }
        else
        {
            query.Answer = 0;
            await producer.SendMessage(query);
        }
    }
    

    public async Task MakeTicketRefund(PayInfo query)
    {
        query.Cost *= -1;
        await repository.Update(query);
    }
}