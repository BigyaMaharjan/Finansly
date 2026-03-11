namespace Finansly.Application.DTOs.Auth;

public record LoginRequestDto
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}