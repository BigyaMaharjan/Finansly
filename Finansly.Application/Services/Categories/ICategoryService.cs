using Finansly.Application.DTOs.Categories;

namespace Finansly.Application.Services.Categories;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllByUserAsync(Guid userId);
    Task<CategoryDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreateCategoryDto dto);
    Task<Guid> UpdateAsync(Guid id, UpdateCategoryDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<CategoryWithTransactionsResultDto> CreateWithTransactionsAsync(CreateCategoryWithTransactionsDto dto);
}