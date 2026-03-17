using AutoMapper;
using ECommerce.API.Configuration;
using ECommerce.API.Data;
using ECommerce.API.Modules.Auth.DTOs;
using ECommerce.API.Modules.Auth.Entities;
using ECommerce.API.Modules.Products.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;

namespace ECommerce.API.Modules.Auth.Services;

public class AuthService : IAuthService
{
    private const string DuplicateEmailMessage = "A user with this email already exists.";
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
        var normalizedEmail = NormalizeEmail(request.Email);
        var emailExists = await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email.ToLower() == normalizedEmail);

        if (emailExists)
        {
            throw new InvalidOperationException(DuplicateEmailMessage);
        }

        var user = _mapper.Map<User>(request);
        var now = DateTime.UtcNow;
        user.Email = normalizedEmail;
        user.PasswordHash = _passwordHasher.HashPassword(request.Password);
        user.CreatedAt = now;
        user.UpdatedAt = now;
        user.Cart = new Cart
        {
            CreatedAt = now,
            UpdatedAt = now
        };

        _dbContext.Users.Add(user);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (IsUniqueEmailViolation(ex))
        {
            throw new InvalidOperationException(DuplicateEmailMessage);
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

    public async Task<AuthResponseDto> AuthenticateAsync(LoginRequestDto request)
    {
        var normalizedEmail = NormalizeEmail(request.Email);
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail);

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

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

    private static bool IsUniqueEmailViolation(DbUpdateException exception) =>
        exception.InnerException is PostgresException postgresException
        && postgresException.SqlState == PostgresErrorCodes.UniqueViolation
        && string.Equals(postgresException.ConstraintName, "IX_Users_Email", StringComparison.Ordinal);
}
