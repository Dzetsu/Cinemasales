using Cinemasales.Entities;
using Cinemasales.Services;
using Cinemasales.Services.TicketServices;
using Microsoft.AspNetCore.Mvc;

namespace Cinemasales.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketController(ITicketService ticket) : ControllerBase
{
    [HttpGet]
    public Task<IEnumerable<Seat>> GetAllSeats()
    {
        return ticket.GetAllSeats();
    }

    [HttpPost]
    public async Task CreateOrder(string seatNumber, string username, string pincode)
    {
        await ticket.CreateOrder(seatNumber, username, pincode);
    }
}