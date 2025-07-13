using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WalletPay.Repositories;
using WalletPay.Services;
using WalletPay.Services.BackgroundService;
using WalletPay.Services.Kafka;

namespace WalletPay;

class Program
{
    private static async Task Main()
    {
        var builder = WebApplication.CreateBuilder();
        
        builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("DefaultConnection")!);
        builder.Services.AddHostedService<ResponseForwarder>();
        builder.Services.AddHostedService<RollbackService>();
        builder.Services.AddSingleton<IPaymentRepository, PaymentRepository>();
        builder.Services.AddSingleton<IPaymentService, PaymentService>();
        builder.Services.AddSingleton<PaymentProducer>();
        
        var app = builder.Build();
        
        await app.RunAsync();
    }
}