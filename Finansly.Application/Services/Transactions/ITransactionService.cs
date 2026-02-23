using Finansly.Application.DTOs.Transactions;

namespace Finansly.Application.Services.Transactions;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetAllByUserAsync(Guid userId);
    Task<TransactionDto?> GetByIdAsync(Guid id);
    Task<TransactionDto> CreateAsync(CreateTransactionDto dto);
    Task UpdateAsync(Guid id, UpdateTransactionDto dto);
    Task DeleteAsync(Guid id);
    Task<decimal> GetTotalByTypeAsync(GetTotalByTypeRequestDto request);
}