namespace conversor.Models;

public class UserForUpdateDTO
{
    public int UserToChangeID { get; set; }
    public String UserName { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public String Email { get; set; }
    public String Password { get; set; }
    public String Plan { get; set; }
}