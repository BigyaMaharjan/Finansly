using Finansly.Domain.Constants;
using Finansly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finansly.Infrastructure.Persistence;

public class FinanslyDbContext : DbContext
{
    public FinanslyDbContext(DbContextOptions<FinanslyDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Max lengths
        modelBuilder.Entity<User>().Property(u => u.Name).HasMaxLength(EntityLengths.User.Name);
        modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(EntityLengths.User.Email);
        modelBuilder.Entity<User>().Property(u => u.PasswordHash).HasMaxLength(EntityLengths.User.PasswordHash);
        modelBuilder.Entity<User>().Property(u => u.Bio).HasMaxLength(EntityLengths.User.Bio);
        modelBuilder.Entity<Category>().Property(c => c.Name).HasMaxLength(EntityLengths.Category.Name);
        modelBuilder.Entity<Transaction>().Property(t => t.Description).HasMaxLength(EntityLengths.Transaction.Description);

        // Unique email
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Decimal precision
        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);

        // Soft delete global filter
        modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Transaction>().HasQueryFilter(x => !x.IsDeleted);
    }
}