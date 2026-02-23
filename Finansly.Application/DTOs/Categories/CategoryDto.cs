using Finansly.Domain.Enums;

namespace Finansly.Application.DTOs.Categories;

public record CategoryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public CategoryType Type { get; init; }
    public string TypeName { get; init; } = null!;
    public Guid UserId { get; init; }
    public DateTime CreatedAt { get; init; }
}