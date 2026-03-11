namespace Finansly.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string? Bio { get; set; }

    // Navigation properties
    public ICollection<Category> Categoriess { get; set; } = new List<Category>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
