using Finansly.Application.Common.Models;
using Finansly.Application.DTOs.Transactions;

namespace Finansly.Application.Services.Transactions;

public interface ITransactionService
{
    Task<PagedResultDto<TransactionDto>> GetPagedAsync(Guid userId, GetTransactionsRequestDto request);
    Task<TransactionDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(Guid userId, CreateTransactionDto dto);
    Task<Guid> UpdateAsync(Guid id, UpdateTransactionDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<decimal> GetTotalByTypeAsync(Guid userId, GetTotalByTypeRequestDto request);
}
