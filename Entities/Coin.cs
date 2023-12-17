using SQLite;

namespace conversor.Entities;

public class Coin
{
    public int Id { get; set; }
    public String Name { get; set; }
    [MaxLength(5)]
    public String Code { get; set; }
    public double Value { get; set; }
    //public String ImageUrl { get; set; }
}