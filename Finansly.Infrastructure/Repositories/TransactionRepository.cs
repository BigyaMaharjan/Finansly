using Finansly.Application.Interfaces.Transactions;
using Finansly.Domain.Entities;
using Finansly.Domain.Enums;
using Finansly.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Finansly.Infrastructure.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(FinanslyDbContext context) : base(context) { }

    public async Task<IEnumerable<Transaction>> GetByUserAsync(Guid userId)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalByTypeAsync(Guid userId, CategoryType type, int month, int year)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Where(t => t.UserId == userId 
                && t.Category.Type == type 
                && t.Date.Month == month 
                && t.Date.Year == year)
            .SumAsync(t => t.Amount);
    }

    public override async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}
