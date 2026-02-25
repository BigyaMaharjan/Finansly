using Finansly.Application.Common.Enums;
using Finansly.Application.Common.Models;
using Finansly.Application.DTOs.Transactions;
using Finansly.Application.Interfaces.Transactions;
using Finansly.Domain.Entities;
using Finansly.Infrastructure.Extensions;
using Finansly.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Finansly.Infrastructure.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(FinanslyDbContext context, TimeProvider timeProvider) : base(context, timeProvider) { }

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

    public async Task<PagedResultDto<TransactionDto>> GetPagedAsync(Guid userId, GetTransactionsRequestDto request, CancellationToken cancellationToken = default)
    {
        // 1. FILTER
        var keyword = request.SearchKeyword?.Trim().ToLower();

        var query = _dbSet
            .Include(t => t.Category)
            .Where(t => t.UserId == userId)
            .WhereIf(request.CategoryId.HasValue,      t => t.CategoryId == request.CategoryId!.Value)
            .WhereIf(request.Type.HasValue,             t => t.Category.Type == request.Type!.Value)
            .WhereIf(request.DateFrom.HasValue,         t => t.Date >= request.DateFrom!.Value)
            .WhereIf(request.DateTo.HasValue,           t => t.Date <= request.DateTo!.Value)
            .WhereIf(!string.IsNullOrWhiteSpace(keyword),
                     t => t.Description.ToLower().Contains(keyword!) ||
                          t.Category.Name.ToLower().Contains(keyword!));

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

        // 3. SORT — default: CreatedAt DESC
        var sortExpression = string.IsNullOrWhiteSpace(request.Sorting)
            ? $"{nameof(TransactionDto.CreatedAt)} DESC"
            : $"{request.Sorting}";

        var sorted = projected.OrderBy(sortExpression)
                              .Skip(request.SkipCount)
                              .Take(request.MaxResultCount);

        // 4. PAGINATE
        var totalCount = await query.CountAsync(cancellationToken);

        // 5. MATERIALIZE
        var items = await sorted.ToListAsync(cancellationToken);

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
