using conversor.Data.Interfaces;
using conversor.Entities;
using conversor.Models;
using Microsoft.EntityFrameworkCore;

namespace conversor.Data.Implementations;

public class UserService : IUserService
{
    private readonly ConversorContext _context;

    public UserService(ConversorContext context)
    {
        _context = context;
    }

    public List<User> GetUsers()
    {
        return _context.Users.ToList();
    }

    public User? GetUser(int id)
    {
        User? user = _context.Users.FirstOrDefault((users) => users.Id == id);

        return user;
    }
    
    public User? GetUserEmail(string email)
    {
        return _context.Users.Include((user) => user.Plan)
            .Include((user) => user.Auth).FirstOrDefault((users) => users.Email.ToLower() == email.ToLower());
    }

    public UserForCheckDTO AddUser(UserForCreationDTO userForCreationDto)
    {
        User user = new()
        {
            Username = userForCreationDto.Username,
            FirstName = userForCreationDto.FirstName,
            LastName = userForCreationDto.LastName,
            Email = userForCreationDto.Email,
            PlanId = 1,
        };

        Auth auth = new()
        {
            Password = userForCreationDto.Password,
            Role = "User"
        };

        var authCreated = _context.Auth.Add(auth);
        _context.SaveChanges();

        user.Auth = authCreated.Entity;
        user.AuthId = authCreated.Entity.Id;

        var userCreated = _context.Users.Add(user);
        _context.SaveChanges();

        return new UserForCheckDTO()
        {
            Email = userForCreationDto.Email,
            FirstName = userForCreationDto.FirstName,
            LastName = userForCreationDto.LastName,
            Username = userForCreationDto.Username,
            Id = userCreated.Entity.Id,
        };
    }

    public void UpdateUser(UserForUpdateDTO userForUpdateDto)
    {
        User? toChange = GetUser(userForUpdateDto.UserId);

        User? userExist = _context.Users.FirstOrDefault((users) => users.Email == userForUpdateDto.Email);

        if (userExist == null)
        {
            throw new Exception("NT - User email already exists");
        }

        toChange.FirstName = userForUpdateDto.FirstName;
        toChange.LastName = userForUpdateDto.LastName;
        toChange.Email = userForUpdateDto.Email;

        try
        {
            _context.Users.Update(toChange);
        }
        catch (Exception)
        {
            throw new Exception("IE - An error occurred while setting the data in the database");
        }

        try
        {
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("IE - An error occurred while saving the data in the database");
        }
    }

    public void DeleteUser(int userId)
    {
        User? toRemove = GetUser(userId);

        try
        {
            _context.Users.Remove(toRemove);
        }
        catch (Exception)
        {
            throw new Exception("IE - An error occurred while setting the data in the database");
        }

        try
        {
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("IE - An error occurred while saving the data in the database");
        }
    }
}