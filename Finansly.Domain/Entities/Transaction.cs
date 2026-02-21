namespace Finansly.Domain.Entities;

public class Transaction : BaseEntity
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = null!;
    public int CategoryId { get; set; }
    public int UserId { get; set; }
    // Navigation properties
    public Category Category { get; set; } = null!;
    public User User { get; set; } = null!;
}