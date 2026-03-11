using Finansly.Application.Common.Interfaces;
using Finansly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finansly.Infrastructure.Persistence;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly FinanslyDbContext _context;
    protected readonly DbSet<T> _dbSet;
    private readonly TimeProvider _timeProvider;

    public BaseRepository(FinanslyDbContext context, TimeProvider timeProvider)
    {
        _context = context;
        _dbSet = context.Set<T>();
        _timeProvider = timeProvider;
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
        entity.CreatedAt = _timeProvider.GetUtcNow().UtcDateTime;
        await _dbSet.AddAsync(entity);
    }

    public virtual void Update(T entity)
    {
        entity.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;
        _dbSet.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;
        _dbSet.Update(entity);
    }
    public async Task SaveChangesAsync()
       => await _context.SaveChangesAsync();
}
