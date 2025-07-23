using System.Diagnostics;
using Cinemasales.Entities;
using Cinemasales.Entities.BookEntities;
using Cinemasales.Entities.PayEntities;
using Cinemasales.Repositories.TicketRepositories;
using Cinemasales.Services.KafkaServices;
using Cinemasales.Settings;
using Microsoft.Extensions.Options;

namespace Cinemasales.Services.TicketServices;

public class TicketService(ITicketRepository ticket, ITicketRepository repository, ProducerService producer, IOptions<ProducerKafkaSettings> options) : ITicketService
{
    private readonly ProducerKafkaSettings _producerConfig = options.Value ?? throw new ArgumentNullException(nameof(options));

    public async Task<IEnumerable<Seat>> GetAllSeats()
    {
        return await ticket.GetAll();
    }

    public async Task CreateOrder(string seatNumber, string username, string pincode)
    {
        try
        {
            var cost = seatNumber[0] switch
            {
                'A' => 300,
                'B' => 500,
                'C' => 750,
                _ => throw new Exception("Invalid seat number")
            };

            var token = Guid.NewGuid().ToString();

            var payMessage = new PayMessage
            {
                Username = username,
                Pincode = pincode,
                Cost = cost,
                Token = token
            };

            var bookMessage = new BookMessage
            {
                SeatNumber = seatNumber,
                Token = token
            };
            
            await producer.SendMessage(payMessage, _producerConfig.PayTopic);
            await producer.SendMessage(bookMessage, _producerConfig.BookTopic);

            var order = new Order
            {
                Username = username,
                SeatNumber = seatNumber,
                Token = token
            };
            
            await repository.Create(order);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}