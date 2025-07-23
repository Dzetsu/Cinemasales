using Cinemasales.Repositories.BookRepository;
using Cinemasales.Repositories.PayRepositories;
using Cinemasales.Repositories.ResultRepositories;
using Cinemasales.Repositories.TicketRepositories;
using Cinemasales.Services.BackgroundServices;
using Cinemasales.Services.KafkaServices;
using Cinemasales.Services.TicketServices;
using Cinemasales.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("DefaultConnection")!);
builder.Services.AddSingleton<ITicketService, TicketService>();
builder.Services.AddSingleton<ITicketRepository, TicketRepository>();
builder.Services.AddSingleton<IPayRepository, PayRepository>();
builder.Services.AddSingleton<IBookRepository, BookRepository>();
builder.Services.AddSingleton<IResultRepository, ResultRepository>();

// Вопрос к Григорию. Словил проблему, веб приложение не запускается, если я внедряю background services
// Нужна помощь, чтобы пофиксить (так как у самого не получилось)
builder.Services.AddHostedService<PayCoordinatorService>();
builder.Services.AddHostedService<BookCoordinatorService>();
builder.Services.AddHostedService<MainCoordinatorService>();

builder.Services.Configure<ConsumerKafkaSettings>(builder.Configuration.GetSection("ConsumerConfig"));
builder.Services.Configure<ProducerKafkaSettings>(builder.Configuration.GetSection("ProducerConfig"));
builder.Services.AddSingleton<ProducerService>();

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