namespace Finansly.Application.DTOs.Categories;

public record CategoryTypeLookupDto 
{
    public string Name { get; init; } = null!;
    public int Value { get; init; }
}