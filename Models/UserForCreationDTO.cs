using System.ComponentModel.DataAnnotations;
using conversor.Entities;

namespace conversor.Models;

public class UserForCreationDTO
{
    public String Username { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public String Email { get; set; }
    [Required, RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$")]
    public String Password { get; set; }
}