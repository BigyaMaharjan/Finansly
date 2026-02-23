using Finansly.Application.DTOs.Categories;
using Finansly.Application.Interfaces.Categories;
using Finansly.Application.Services.Categories;
using Finansly.Domain.Entities;

namespace Finansly.Infrastructure.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllByUserAsync(Guid userId)
    {
        var categories = await _repository.GetByUserAsync(userId);
        return categories.Select(MapToDto);
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id)
    {
        var category = await _repository.GetByIdAsync(id);
        return category is null ? null : MapToDto(category);
    }

    public async Task<Guid> CreateAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Type = dto.Type,
            UserId = dto.UserId
        };

        await _repository.AddAsync(category);
        await _repository.SaveChangesAsync();

        return category.Id;
    }

    public async Task<Guid> UpdateAsync(Guid id, UpdateCategoryDto dto)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category is null)
            throw new KeyNotFoundException($"Category with id {id} not found.");

        category.Name = dto.Name;
        category.Type = dto.Type;

        _repository.Update(category);
        await _repository.SaveChangesAsync();
        
        return category.Id;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category is null)
            return false;

        _repository.Delete(category);
        await _repository.SaveChangesAsync();
        
        return true;
    }

    private static CategoryDto MapToDto(Category c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Type = c.Type,
        TypeName = c.Type.ToString(),
        UserId = c.UserId,
        CreatedAt = c.CreatedAt
    };
}
