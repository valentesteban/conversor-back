namespace conversor.Models;

public class UserForCheckDTO
{
    public int Id { get; set; }
    public String Username { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public String Email { get; set; }  
    
    public PlanForCheckDTO Plan { get; set; }
}