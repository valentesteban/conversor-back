using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using conversor.Data.Interfaces;
using conversor.Entities;
using conversor.Models;
using Microsoft.IdentityModel.Tokens;

namespace conversor.Data.Implementations;

public class AuthService : IAuthService
{
    private readonly ConversorContext _context;
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(ConversorContext context, IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _config = config;
        _httpContextAccessor = httpContextAccessor;
    }

    public Auth Authenticate(UserForLoginDTO userForLoginDto)
    {
        var user = _context.Users.FirstOrDefault(x => x.Username.ToLower() == userForLoginDto.Username.ToLower());
        
         if (user == null)
         {
             return null;
         }
         
        var password = _context.Auth.FirstOrDefault(x => x.Password == userForLoginDto.Password && x.Id == user.Id);
       
        if (user != null && password != null)
        {
            return password;
        }

        return null;
    }

    public string GenerateToken(Auth auth)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim("userId",auth.Id.ToString()),
            new Claim("role",auth.Role)
        };
        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);    
    }
    
    public Auth GetCurrentUser()
    {
        var claimPrincipal = _httpContextAccessor.HttpContext.User;
        
        if (claimPrincipal == null)
        {
            return null;
        }
        
        var identity = claimPrincipal.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var userClaims = identity.Claims;
            
            if (userClaims == null)
            {
                return null;
            }

            if (userClaims.FirstOrDefault(x => x.Type == "userId") == null)
            {
                return null;
            }
            return new Auth
            {
                Id = int.Parse(userClaims.FirstOrDefault(x => x.Type == "userId")?.Value),
                Role = userClaims.FirstOrDefault(x => x.Type == "role")?.Value
            };
        }
        return null;
    }

    public bool SameUserRequest(int userId)
    {
        var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var userClaims = identity.Claims;
            
            if (userClaims.FirstOrDefault(x => x.Type == "role")?.Value.ToLower() == "admin")
            {
                return true;
            }
            
            if (userClaims.FirstOrDefault(x => x.Type == "userId")?.Value != null)

            return int.Parse(userClaims.FirstOrDefault(x => x.Type == "userId")?.Value) == userId;
        }
        return false;
    }
}