namespace conversor.Models;

public class ExchangeForCreationDTO
{
    public int UserId { get; set; }
    public int FromCoinId { get; set; }
    public int ToCoinId { get; set; }
    public double Amount { get; set; }
}