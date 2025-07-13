using BookSeat;
using BookSeat.Repositories;
using BookSeat.Services;
using BookSeat.Services.BackgroundService;
using BookSeat.Services.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main()
    {
        var builder = WebApplication.CreateBuilder();
        
        builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("DefaultConnection")!);
        builder.Services.AddHostedService<ResponseForwarder>();
        builder.Services.AddHostedService<RollbackService>();
        builder.Services.AddSingleton<IBookedRepository, BookedRepository>();
        builder.Services.AddSingleton<IBookedService, BookedService>();
        builder.Services.AddSingleton<BookedProducer>();
        
        var app = builder.Build();
        
        await app.RunAsync();
    }
}

