namespace Finansly.Application.DTOs.Users;

public record UpdateUserDto
{
    public string Name { get; init; } = null!;
    public DateTime? DateOfBirth { get; init; }
    public string? Bio { get; init; }
}
