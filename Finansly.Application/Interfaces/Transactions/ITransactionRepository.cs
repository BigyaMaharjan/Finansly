using Finansly.Application.Common.Interfaces;
using Finansly.Application.Common.Models;
using Finansly.Application.DTOs.Transactions;
using Finansly.Domain.Entities;

namespace Finansly.Application.Interfaces.Transactions;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetByUserAsync(Guid userId);
    Task<decimal> GetTotalByTypeAsync(Guid userId, GetTotalByTypeRequestDto request);
    Task<PagedResultDto<TransactionDto>> GetPagedAsync(Guid userId, GetTransactionsRequestDto request, CancellationToken cancellationToken = default);
}
