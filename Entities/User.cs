using SQLite;
using SQLiteNetExtensions.Attributes;

namespace conversor.Entities;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id {get; set;}
    public String Username {get; set;}
    public String FirstName {get; set;}
    public String LastName {get; set;}
    public String Email {get; set;}
    [OneToMany (CascadeOperations = CascadeOperation.CascadeRead)]
    public List<CoinExchange> Exchanges { get; set; } = new();
    public int AuthId { get; set; }
    public Auth? Auth { get; set; }
    public int PlanId { get; set; }
    public Plan Plan { get; set; } = null!;
}