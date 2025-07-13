namespace Cinemasales.Entities;

public class Result
{
    public int Id { get; set; }
    public string Token { get; set; }
    public short PayStatus { get; set; }
    public short BookStatus { get; set; }
    public short ResultStatus { get; set; }
    public string SeatNumber { get; set; }
    public string Username { get; set; }
    public int Cost { get; set; }
    public string Pincode { get; set; }
}