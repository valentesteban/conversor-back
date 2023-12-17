using conversor.Entities;
using conversor.Models;

namespace conversor.Data.Interfaces;

public interface IAuthService
{
    public Auth Authenticate(UserForLoginDTO userForLoginDto);
    public string GenerateToken(Auth auth);
    public Auth getCurrentUser();
    public Boolean isSameUserRequest(int userId);
}