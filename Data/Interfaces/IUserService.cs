using conversor.Entities;
using conversor.Models;

namespace conversor.Data.Interfaces;

public interface IUserService
{
    public List<User> GetUsers();
    public User? GetUser(int id);
    public User? GetUser(String email);
    public void AddUser(UserForCreationDTO userForCreationDto);
    public void UpdateUser(UserForUpdateDTO userForUpdateDto);
    public void DeleteUser(int userId);
}