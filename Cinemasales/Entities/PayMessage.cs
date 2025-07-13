namespace Cinemasales.Entities;

public class PayMessage
{
    public string Username { get; set; }
    public string Pincode { get; set; }
    public int Cost  { get; set; }
    public string Token { get; set; }
    public int Answer { get; set; }
}