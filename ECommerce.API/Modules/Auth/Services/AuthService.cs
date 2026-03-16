using AutoMapper;
using ECommerce.API.Configuration;
using ECommerce.API.Data;
using ECommerce.API.Modules.Auth.DTOs;
using ECommerce.API.Modules.Auth.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ECommerce.API.Modules.Auth.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;
    private readonly JwtSettings _settings;

    public AuthService(
        ApplicationDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        IMapper mapper,
        IOptions<JwtSettings> settings)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
        _settings = settings.Value;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var existing = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (existing is not null)
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        var user = _mapper.Map<User>(request);
        user.PasswordHash = _passwordHasher.HashPassword(request.Password);
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var token = _jwtTokenService.GenerateToken(user);
        return new AuthResponseDto
        {
            Token = token,
            ExpiresInSeconds = (int)TimeSpan.FromMinutes(_settings.ExpiryMinutes).TotalSeconds,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString()
        };
    }

    public async Task<AuthResponseDto> AuthenticateAsync(LoginRequestDto request)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user is null || !_passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("User is not active.");
        }

        var token = _jwtTokenService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresInSeconds = (int)TimeSpan.FromMinutes(_settings.ExpiryMinutes).TotalSeconds,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString()
        };
    }
}
