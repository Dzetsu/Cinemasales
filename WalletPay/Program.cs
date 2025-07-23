using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WalletPay.Repositories;
using WalletPay.Services.BackgroundServices;
using WalletPay.Services.KafkaServices;
using WalletPay.Services.PaymentServices;
using WalletPay.Settings;

namespace WalletPay;

public static class Program
{
    private static async Task Main()
    {
        var builder = WebApplication.CreateBuilder();
        
        builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("DefaultConnection")!);
        builder.Services.AddHostedService<ResponseForwarderService>();
        builder.Services.AddHostedService<RollbackService>();
        builder.Services.AddSingleton<IPaymentRepository, PaymentRepository>();
        builder.Services.AddSingleton<IPaymentService, PaymentService>();
        builder.Services.AddSingleton<PaymentProducer>();
        builder.Services.Configure<ConsumerKafkaSettings>(builder.Configuration.GetSection("ConsumerConfig"));
        builder.Services.Configure<ProducerKafkaSettings>(builder.Configuration.GetSection("ProducerConfig"));
        
        var app = builder.Build();
        
        await app.RunAsync();
    }
}