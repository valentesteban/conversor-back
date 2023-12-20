using conversor.Entities;
using conversor.Models;

namespace conversor.Data.Interfaces;

public interface IAuthService
{
    public Auth Authenticate(UserForLoginDTO userForLoginDto);
    public string GenerateToken(Auth auth);
    public Auth GetCurrentUser();
    public Boolean SameUserRequest(int userId);
}