using Finansly.Application.Common.Interfaces;
using Finansly.Domain.Entities;
using Finansly.Domain.Enums;

namespace Finansly.Application.Interfaces.Transactions;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetByUserAsync(Guid userId);
    Task<decimal> GetTotalByTypeAsync(Guid userId, CategoryType type, int month, int year);
}