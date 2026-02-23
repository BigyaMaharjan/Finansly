namespace Finansly.Application.DTOs.Auth;

public record LoginRequestDto
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}

public record AuthResponseDto
{
    public string Token { get; init; } = null!;
    public DateTime ExpiresAt { get; init; }
    public Guid UserId { get; init; }
    public string Name { get; init; } = null!;
}
