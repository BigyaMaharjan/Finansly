using Finansly.Application.Common.Interfaces;
using Finansly.Domain.Entities;
using Finansly.Domain.Enums;

namespace Finansly.Application.Interfaces.Transactions;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetByUserAsync(int userId);
    Task<decimal> GetTotalByTypeAsync(int userId, CategoryType type, int month, int year);
}