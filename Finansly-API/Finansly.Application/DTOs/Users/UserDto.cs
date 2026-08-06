namespace Finansly.Application.DTOs.Users;

public record UserDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public DateTime DateOfBirth { get; init; }
    public string? Bio { get; init; }
    public DateTime CreatedAt { get; init; }
}
