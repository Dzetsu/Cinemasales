namespace Cinemasales.Entities;

public class Result
{
    public int Id { get; init; }
    public string Token { get; init; }
    public short PayStatus { get; init; }
    
    public short BookStatus { get; init; }
    public short ResultStatus { get; init; }
    public string SeatNumber { get; init; }
    public string Username { get; init; }
    public int Cost { get; init; }
    public string Pincode { get; init; }
}