using ECommerce.API.Modules.Auth.DTOs;

namespace ECommerce.API.Modules.Auth.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto> AuthenticateAsync(LoginRequestDto request);
}
