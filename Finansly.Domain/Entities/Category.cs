namespace Finansly.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;
    public CategoryType Type { get; set; }
    public int UserId { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
