namespace Cinemasales.Entities.PayEntities;

public class PayResult
{
    public string Token { get; init; }
    public short Answer { get; init; }
    public string Username { get; init; }
    public int TicketCost { get; init; }
    public string Pincode { get; init; }
}