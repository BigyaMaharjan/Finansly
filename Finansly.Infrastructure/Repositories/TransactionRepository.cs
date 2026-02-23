using Finansly.Application.Common.Enums;
using Finansly.Application.Common.Models;
using Finansly.Application.DTOs.Transactions;
using Finansly.Application.Interfaces.Transactions;
using Finansly.Domain.Entities;
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

    public async Task<decimal> GetTotalByTypeAsync(Guid userId, GetTotalByTypeRequestDto request)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Where(t => t.UserId == userId
                && t.Category.Type == request.Type
                && t.Date.Month == request.Month
                && t.Date.Year == request.Year)
            .SumAsync(t => t.Amount);
    }

    public async Task<PagedResultDto<TransactionDto>> GetPagedAsync(Guid userId, GetTransactionsRequestDto request)
    {
        // 1. FILTER
        var query = _dbSet
            .Include(t => t.Category)
            .Where(t => t.UserId == userId);

        if (request.CategoryId.HasValue)
            query = query.Where(t => t.CategoryId == request.CategoryId.Value);

        if (request.Type.HasValue)
            query = query.Where(t => t.Category.Type == request.Type.Value);

        if (request.DateFrom.HasValue)
            query = query.Where(t => t.Date >= request.DateFrom.Value);

        if (request.DateTo.HasValue)
            query = query.Where(t => t.Date <= request.DateTo.Value);

        // 2. PROJECT
        var projected = query.Select(t => new TransactionDto
        {
            Id = t.Id,
            Amount = t.Amount,
            Date = t.Date,
            Description = t.Description,
            CategoryId = t.CategoryId,
            CategoryName = t.Category.Name,
            UserId = t.UserId,
            CreatedAt = t.CreatedAt
        });

        // 3. SORT
        var sorted = (request.Sorting?.ToLower(), request.SortType) switch
        {
            ("amount",      SortType.Descending) => projected.OrderByDescending(t => t.Amount),
            ("amount",      _)                        => projected.OrderBy(t => t.Amount),
            ("description", SortType.Descending) => projected.OrderByDescending(t => t.Description),
            ("description", _)                        => projected.OrderBy(t => t.Description),
            (_,             SortType.Descending) => projected.OrderByDescending(t => t.Date),
            _                                         => projected.OrderBy(t => t.Date)
        };

        // 4. PAGINATE
        var totalCount = await query.CountAsync();
        var paged = sorted.Skip(request.SkipCount).Take(request.MaxResultCount);

        // 5. MATERIALIZE
        var items = await paged.ToListAsync();

        return new PagedResultDto<TransactionDto>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public override async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}
