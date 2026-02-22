namespace Finansly.Application.DTOs.Transactions;

public class TransactionDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = null!;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateTransactionDto
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = null!;
    public int CategoryId { get; set; }
    public int UserId { get; set; }
}

public class UpdateTransactionDto
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = null!;
    public int CategoryId { get; set; }
}
