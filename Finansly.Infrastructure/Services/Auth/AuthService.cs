using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Finansly.Application.Common.Interfaces;
using Finansly.Application.DTOs.Auth;
using Finansly.Application.Interfaces.Users;
using Finansly.Application.Services.Auth;
using Finansly.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Finansly.Infrastructure.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IConfiguration _configuration;
    private readonly TimeProvider _timeProvider;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IConfiguration configuration, TimeProvider timeProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
        _timeProvider = timeProvider;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user is null || !_passwordHasher.Verify(dto.Password, user.PasswordHash))
            return null;

        return BuildToken(user);
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
    {
        if (await _userRepository.ExistsEmailAsync(dto.Email))
            throw new InvalidOperationException($"Email '{dto.Email}' is already registered.");

        var user = new User
        {
            Name = dto.Name.Trim(),
            Email = dto.Email.Trim(),
            PasswordHash = _passwordHasher.Hash(dto.Password),
            DateOfBirth = dto.DateOfBirth
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return BuildToken(user);
    }

    private AuthResponseDto BuildToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);
        var expiresAt = _timeProvider.GetUtcNow().UtcDateTime.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"]!));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthResponseDto
        {
            Token = tokenHandler.WriteToken(token),
            ExpiresAt = expiresAt,
            UserId = user.Id,
            Name = user.Name
        };
    }
}
