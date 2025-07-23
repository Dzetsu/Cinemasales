using WalletPay.Entities;
using WalletPay.Repositories;
using WalletPay.Services.KafkaServices;

namespace WalletPay.Services.PaymentServices;

public class PaymentService(IPaymentRepository repository, PaymentProducer producer) : IPaymentService
{
    public async Task PayTicket(PayQuery query)
    {
        var payAnswer = new PayAnswer
        {
            Token = query.Token,
            Username = query.Username,
            TicketCost = query.Cost,
            Pincode = query.Pincode
        };
        
        var balance = await repository.GetBalance(query);

        if (balance >= query.Cost)
        {
            await repository.UpdateBalance(query);
            payAnswer.Answer = 1;
        }
        else
            payAnswer.Answer = 0;
        
        await producer.SendMessage(payAnswer);
    }
    

    public async Task RefundTicket(PayQuery query)
    {
        query.Cost *= -1;
        await repository.UpdateBalance(query);
        query.Cost *= -1;
    }
}