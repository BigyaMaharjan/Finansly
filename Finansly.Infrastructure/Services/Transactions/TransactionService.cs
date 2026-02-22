using Finansly.Application.DTOs.Transactions;
using Finansly.Application.Interfaces.Transactions;
using Finansly.Application.Services.Transactions;
using Finansly.Domain.Entities;
using Finansly.Domain.Enums;

namespace Finansly.Infrastructure.Services.Transactions;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repository;

    public TransactionService(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TransactionDto>> GetAllByUserAsync(int userId)
    {
        var transactions = await _repository.GetByUserAsync(userId);
        return transactions.Select(MapToDto);
    }

    public async Task<TransactionDto?> GetByIdAsync(int id)
    {
        var transaction = await _repository.GetByIdAsync(id);
        return transaction is null ? null : MapToDto(transaction);
    }

    public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto)
    {
        var transaction = new Transaction
        {
            Amount = dto.Amount,
            Date = dto.Date,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            UserId = dto.UserId
        };

        await _repository.AddAsync(transaction);
        await _repository.SaveChangesAsync();
        
        return MapToDto(transaction);
    }

    public async Task UpdateAsync(int id, UpdateTransactionDto dto)
    {
        var transaction = await _repository.GetByIdAsync(id);
        if (transaction is null)
            throw new KeyNotFoundException($"Transaction with id {id} not found");

        transaction.Amount = dto.Amount;
        transaction.Date = dto.Date;
        transaction.Description = dto.Description;
        transaction.CategoryId = dto.CategoryId;

        _repository.Update(transaction);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var transaction = await _repository.GetByIdAsync(id);
        if (transaction is null)
            throw new KeyNotFoundException($"Transaction with id {id} not found");

        _repository.Delete(transaction);
        await _repository.SaveChangesAsync();
    }

    public async Task<decimal> GetTotalByTypeAsync(int userId, CategoryType type, int month, int year)
    {
        return await _repository.GetTotalByTypeAsync(userId, type, month, year);
    }

    private static TransactionDto MapToDto(Transaction t) => new()
    {
        Id = t.Id,
        Amount = t.Amount,
        Date = t.Date,
        Description = t.Description,
        CategoryId = t.CategoryId,
        CategoryName = t.Category?.Name ?? string.Empty,
        UserId = t.UserId,
        CreatedAt = t.CreatedAt
    };
}
