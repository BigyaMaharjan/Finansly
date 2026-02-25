using Finansly.Application.Common.Models;
using Finansly.Application.DTOs.Transactions;
using Finansly.Application.Interfaces.Categories;
using Finansly.Application.Interfaces.Transactions;
using Finansly.Application.Services.Transactions;
using Finansly.Domain.Entities;

namespace Finansly.Infrastructure.Services.Transactions;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repository;
    private readonly ICategoryRepository _categoryRepository;

    public TransactionService(ITransactionRepository repository, ICategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
    }

    public async Task<PagedResultDto<TransactionDto>> GetPagedAsync(Guid userId, GetTransactionsRequestDto request, CancellationToken cancellationToken = default)
    {
        return await _repository.GetPagedAsync(userId, request, cancellationToken);
    }

    public async Task<TransactionDto?> GetByIdAsync(Guid id)
    {
        var transaction = await _repository.GetByIdAsync(id);
        return transaction is null ? null : MapToDto(transaction);
    }

    public async Task<Guid> CreateAsync(Guid userId, CreateTransactionDto dto)
    {
        await ValidateCategoryOwnershipAsync(dto.CategoryId, userId);

        var transaction = new Transaction
        {
            Amount = dto.Amount,
            Date = dto.Date,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            UserId = userId
        };

        await _repository.AddAsync(transaction);
        await _repository.SaveChangesAsync();

        return transaction.Id;
    }

    public async Task<Guid> UpdateAsync(Guid id, Guid userId, UpdateTransactionDto dto)
    {
        var transaction = await _repository.GetByIdAsync(id);
        if (transaction is null)
            throw new KeyNotFoundException($"Transaction with id {id} not found");

        if (transaction.UserId != userId)
            throw new UnauthorizedAccessException("You do not have permission to update this transaction.");

        await ValidateCategoryOwnershipAsync(dto.CategoryId, userId);

        transaction.Amount = dto.Amount;
        transaction.Date = dto.Date;
        transaction.Description = dto.Description;
        transaction.CategoryId = dto.CategoryId;

        _repository.Update(transaction);
        await _repository.SaveChangesAsync();

        return transaction.Id;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var transaction = await _repository.GetByIdAsync(id);
        if (transaction is null)
            return false;

        _repository.Delete(transaction);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<decimal> GetTotalByTypeAsync(Guid userId, GetTotalByTypeRequestDto request)
    {
        return await _repository.GetTotalByTypeAsync(userId, request);
    }

    private async Task ValidateCategoryOwnershipAsync(Guid categoryId, Guid userId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category is null)
            throw new KeyNotFoundException($"Category with id {categoryId} not found.");
        if (category.UserId != userId)
            throw new UnauthorizedAccessException("The specified category does not belong to you.");
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
