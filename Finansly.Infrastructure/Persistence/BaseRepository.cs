using Finansly.Application.Common.Interfaces;
using Finansly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finansly.Infrastructure.Persistence;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly FinanslyDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(FinanslyDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _dbSet.AddAsync(entity);
    }

    public virtual void Update(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
    }
    public async Task SaveChangesAsync()
       => await _context.SaveChangesAsync();
}
