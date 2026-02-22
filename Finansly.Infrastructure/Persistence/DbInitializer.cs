using Finansly.Domain.Entities;
using Finansly.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace Finansly.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(FinanslyDbContext context)
    {
        if (await context.Users.AnyAsync(u => u.Email == "superUser@ledgerly.com"))
            return; // Already seeded

        var hasher = new PasswordHasher();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Super Admin",
            Email = "superUser@ledgerly.com",
            DateOfBirth = DateTime.UtcNow,
            PasswordHash = hasher.Hash("1q2w3E*")
        };

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }
}
