namespace Finansly.Application.DTOs.Transactions;

public record TransactionDto
{
    public Guid Id { get; init; }
    public decimal Amount { get; init; }
    public DateTime Date { get; init; }
    public string Description { get; init; } = null!;
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = null!;
    public Guid UserId { get; init; }
    public DateTime CreatedAt { get; init; }
}