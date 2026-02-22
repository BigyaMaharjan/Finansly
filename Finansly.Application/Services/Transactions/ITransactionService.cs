using Finansly.Application.DTOs.Transactions;

namespace Finansly.Application.Services.Transactions;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetAllByUserAsync(int userId);
    Task<TransactionDto?> GetByIdAsync(int id);
    Task<TransactionDto> CreateAsync(CreateTransactionDto dto);
    Task UpdateAsync(int id, UpdateTransactionDto dto);
    Task DeleteAsync(int id);
    Task<decimal> GetTotalByTypeAsync(int userId, Domain.Enums.CategoryType type, int month, int year);
}
