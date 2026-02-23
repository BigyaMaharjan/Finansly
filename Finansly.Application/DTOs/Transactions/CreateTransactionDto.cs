namespace Finansly.Application.DTOs.Transactions;

public record CreateTransactionDto
{
    public decimal Amount { get; init; }
    public DateTime Date { get; init; }
    public string Description { get; init; } = null!;
    public Guid CategoryId { get; init; }
}