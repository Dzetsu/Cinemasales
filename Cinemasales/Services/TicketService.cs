using System.Diagnostics;
using System.Text.Json;
using Cinemasales.Entities;
using Cinemasales.Repositories;
using Cinemasales.Repositories.Interfaces;
using Cinemasales.Services.Kafka;
using Confluent.Kafka;

namespace Cinemasales.Services;

public class TicketService(ITicketRepository ticket, KafkaProducer producer, ITicketRepository repository) : ITicketService
{
    public async Task<IEnumerable<Seat>> GetAllSeats()
    {
        return await ticket.GetAll();
    }

    public async Task CreateOrder(string seatNumber, string username, string pincode)
    {
        try
        {
            var cost = 1;
            switch (seatNumber[0])
            {
                case 'A':
                    cost = 300;
                    break;
                case 'B':
                    cost = 500;
                    break;
                case 'C':
                    cost = 750;
                    break;
                default:
                    throw new Exception("Invalid seat number");
            }
            
            var token = Guid.NewGuid().ToString();

            var payMessage = new PayMessage()
            {
                Username = username,
                Pincode = pincode,
                Cost = cost,
                Token = token,
                Answer = 1
            };

            var bookMessage = new BookMessage()
            {
                SeatNumber = seatNumber,
                Token = token,
                Answer = 1
            };
            
            Console.WriteLine(cost);
            
            await KafkaProducer.SendMessage(payMessage, "PayForSeat");
            await KafkaProducer.SendMessage(bookMessage, "BookSeat");

            Order order = new Order()
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