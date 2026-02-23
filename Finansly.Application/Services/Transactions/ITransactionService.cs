using Finansly.Application.DTOs.Transactions;

namespace Finansly.Application.Services.Transactions;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetAllByUserAsync(Guid userId);
    Task<TransactionDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreateTransactionDto dto);
    Task<Guid> UpdateAsync(Guid id, UpdateTransactionDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<decimal> GetTotalByTypeAsync(GetTotalByTypeRequestDto request);
}