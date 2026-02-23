using Finansly.Domain.Enums;

namespace Finansly.Application.DTOs.Categories;

public record CreateCategoryWithTransactionsDto
{
    public string Name { get; init; } = null!;
    public CategoryType Type { get; init; }
    public Guid UserId { get; init; }
    public List<CreateTransactionForCategoryDto> Transactions { get; init; } = [];
}
