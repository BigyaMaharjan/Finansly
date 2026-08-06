namespace Finansly.Application.DTOs.Categories;

public record CreateTransactionForCategoryDto
{
    public decimal Amount { get; init; }
    public DateTime Date { get; init; }
    public string Description { get; init; } = null!;
}
