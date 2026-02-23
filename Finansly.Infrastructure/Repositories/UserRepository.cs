using Finansly.Application.Interfaces.Users;
using Finansly.Domain.Entities;
using Finansly.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Finansly.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(FinanslyDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
    }
}
