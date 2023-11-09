using IbgeDesafio.Api.Models;

namespace IbgeDesafio.Api.Auth;

public interface IJwtService
{
    string GenerateToken(User user);
}