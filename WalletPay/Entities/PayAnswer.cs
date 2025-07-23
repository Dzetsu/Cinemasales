namespace WalletPay.Entities;

public class PayAnswer
{
    public string Token { get; set; }
    public short Answer { get; set; }
    public string Username { get; set; }
    public int TicketCost { get; set; }
    public string Pincode { get; set; }
}