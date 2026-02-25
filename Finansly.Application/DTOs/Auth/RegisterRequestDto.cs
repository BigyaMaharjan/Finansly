namespace Finansly.Application.DTOs.Auth;

public record RegisterRequestDto
{
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public DateTime DateOfBirth { get; init; }
}