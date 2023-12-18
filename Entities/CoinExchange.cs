using SQLite;
using SQLiteNetExtensions.Attributes;

namespace conversor.Entities;

public class CoinExchange
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [ManyToOne (CascadeOperations = CascadeOperation.CascadeRead)]
    public CoinExchange FromCoin { get; set; } = null!;

    [ManyToOne (CascadeOperations = CascadeOperation.CascadeRead)]
    public CoinExchange ToCoin { get; set; } = null!;
    public DateTime Date { get; set; }
    public double Amount { get; set; }
    [ForeignKey(typeof(User))]
    public int UserId { get; set; }
}