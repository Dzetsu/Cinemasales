using BookSeat.Repositories;
using BookSeat.Services.BackgroundServices;
using BookSeat.Services.BookedServices;
using BookSeat.Services.KafkaServices;
using Cinemasales.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ConsumerKafkaSettings = BookSeat.Settings.ConsumerKafkaSettings;
using ProducerKafkaSettings = BookSeat.Settings.ProducerKafkaSettings;

namespace BookSeat;

internal static class Program
{
    private static async Task Main()
    {
        var builder = WebApplication.CreateBuilder();
        
        builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("DefaultConnection")!);
        builder.Services.AddHostedService<ResponseForwarder>();
        builder.Services.AddHostedService<RollbackService>();
        builder.Services.AddSingleton<IBookedRepository, BookedRepository>();
        builder.Services.AddSingleton<IBookedService, BookedService>();
        builder.Services.AddSingleton<BookedProducer>();
        builder.Services.Configure<ConsumerKafkaSettings>(builder.Configuration.GetSection("ConsumerConfig"));
        builder.Services.Configure<ProducerKafkaSettings>(builder.Configuration.GetSection("ProducerConfig"));
        
        var app = builder.Build();
        
        await app.RunAsync();
    }
}