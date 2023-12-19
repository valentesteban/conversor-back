using conversor.Data.Interfaces;
using conversor.Entities;
using conversor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conversor.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userContext;
    private readonly IAuthService _authService;

    public UserController(IUserService _userContext, IAuthService _authService)
    {
        this._userContext = _userContext;
        this._authService = _authService;
    }

    [Route("all")]
    [Authorize(Roles = "admin")]
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_userContext.GetUsers());
    }


    [HttpGet("{userId}")]
    public ActionResult<User> GetUser(int userId)
    {
        try
        {
            if (_authService.getCurrentUser() == null)
            {
                return Unauthorized();
            }

            if (_authService.getCurrentUser().Id == userId || _authService.getCurrentUser().Role == "admin")
            {
                return Ok(_userContext.GetUser(userId));
            }

            return Unauthorized();
        }
        catch (Exception)
        {
            return Problem();
        }
    }
    
    [HttpPost]
    public ActionResult<User> PostUser(UserForCreationDTO userForCreationDto)
    {
        try
        {
            _userContext.AddUser(userForCreationDto);
            return Ok("User created successfully");
        }
        catch (Exception)
        {
            return Problem();
        }
    }

    [HttpPut]
    [Authorize(Roles = "admin")]
    public ActionResult<User> PutUser(UserForUpdateDTO userForUpdateDto)
    {
        try
        {
            _userContext.UpdateUser(userForUpdateDto);
            return Ok("User updated successfully");
        }
        catch (Exception)
        {
            return Problem();
        }
    }

    [HttpDelete("{userId}")]
    [Authorize(Roles = "admin")]
    public ActionResult<User> DeleteUser(int userId)
    {
        try
        {
            _userContext.DeleteUser(userId);
            return Ok("User deleted successfully");
        }
        catch (Exception)
        {
            return Problem();
        }
    }
}