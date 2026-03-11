namespace Finansly.Application.DTOs.Categories;

public record CategoryWithTransactionsResultDto
{
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = null!;
    public List<Guid> TransactionIds { get; init; } = [];
    public int TransactionCount { get; init; }
}
