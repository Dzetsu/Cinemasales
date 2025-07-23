namespace WalletPay.Entities;

public class PayQuery
{
    public string Username { get; init; }
    public string Pincode { get; init; }
    public int Cost  { get; set; }
    public string Token { get; init; }
}