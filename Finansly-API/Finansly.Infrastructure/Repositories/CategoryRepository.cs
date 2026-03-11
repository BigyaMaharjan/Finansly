using Finansly.Application.Interfaces.Categories;
using Finansly.Domain.Entities;
using Finansly.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Finansly.Infrastructure.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(FinanslyDbContext context, TimeProvider timeProvider) : base(context, timeProvider) { }

    public async Task<IEnumerable<Category>> GetByUserAsync(Guid userId)
    {
        return await _dbSet
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public override async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
