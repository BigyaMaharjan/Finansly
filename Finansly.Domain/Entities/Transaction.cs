namespace Finansly.Domain.Entities;

public class Transaction : BaseEntity
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public Guid UserId { get; set; }
    // Navigation properties
    public Category Category { get; set; } = null!;
    public User User { get; set; } = null!;
}