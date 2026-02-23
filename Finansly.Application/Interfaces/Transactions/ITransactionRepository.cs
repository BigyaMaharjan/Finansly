using Finansly.Application.Common.Interfaces;
using Finansly.Application.DTOs.Transactions;
using Finansly.Domain.Entities;

namespace Finansly.Application.Interfaces.Transactions;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetByUserAsync(Guid userId);
    Task<decimal> GetTotalByTypeAsync(GetTotalByTypeRequestDto request);
}