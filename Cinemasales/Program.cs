using Cinemasales.Entities;
using Cinemasales.Repositories;
using Cinemasales.Repositories.Interfaces;
using Cinemasales.Services;
using Cinemasales.Services.BackgroundServices;
using Cinemasales.Services.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("DefaultConnection")!);
builder.Services.AddSingleton<ITicketService, TicketService>();
builder.Services.AddSingleton<ITicketRepository, TicketRepository>();
builder.Services.AddSingleton<IResultRepository<PayMessage>, PayResultRepository>();
builder.Services.AddSingleton<IResultRepository<BookMessage>, BookResultRepository>();
builder.Services.AddSingleton<IMainResultRepository, MainResultRepository>();
builder.Services.AddSingleton<KafkaProducer>();
builder.Services.AddHostedService<PayCoordinator>();
builder.Services.AddHostedService<BookCoordinator>();
builder.Services.AddHostedService<MainCoordinator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();