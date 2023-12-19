namespace conversor.Entities;

public class Plan
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Limit { get; set; }
    public List<User> Users { get; } = new();
}