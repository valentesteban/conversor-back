using conversor.Data.Interfaces;
using conversor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conversor.Controllers;
[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public ActionResult Register(UserForCreationDTO userForCreationDto)
    {
        var userEmail = _userService.GetUserEmail(userForCreationDto.Email);
        
        if (userEmail != null)
        {
            return Conflict(new
            {
                error = "Email already exists"
            });
        }

        var user = _userService.AddUser(userForCreationDto);

        return Ok(user);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult Login(UserForLoginDTO userForLoginDto)
    {
        var user = _authService.Authenticate(userForLoginDto);
        
        if (user != null)
        {
            var token = _authService.GenerateToken(user);
            var userId = user.Id;
            
            var response = new
            {
                token, userId
            };
            
            return Ok(response);
        }
        
        return NotFound("Username or password is incorrect");
    }
}