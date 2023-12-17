using SQLite;
using SQLiteNetExtensions.Attributes;

namespace conversor.Entities;

public class Auth{
    [PrimaryKey]
    [ForeignKey(typeof(User))]
    public int Id {get; set;}
    public User User { get; set; } = null!;
    public String Password {get; set;} 
    public String Role { get; set; }
}