using conversor.Entities;
using conversor.Models;

namespace conversor.Data.Interfaces;

public interface IUserService
{
    public List<User> GetUsers();
    public User? GetUser(int id);
    public User? GetUserEmail(string email);
    public UserForCheckDTO AddUser(UserForCreationDTO userForCreationDto);
    public void UpdateUser(UserForUpdateDTO userForUpdateDto);
    public void DeleteUser(int userId);
}