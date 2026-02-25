namespace Finansly.Application.DTOs.Auth;

public record AuthResponseDto
{
    public string Token { get; init; } = null!;
    public DateTime ExpiresAt { get; init; }
    public Guid UserId { get; init; }
    public string Name { get; init; } = null!;
}