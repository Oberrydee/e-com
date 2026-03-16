using ECommerce.API.Modules.Auth.Entities;

namespace ECommerce.API.Modules.Auth.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
