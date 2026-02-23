using Finansly.Domain.Enums;

namespace Finansly.Application.DTOs.Categories;

public record CreateCategoryDto
{
    public string Name { get; init; } = null!;
    public CategoryType Type { get; init; }
}