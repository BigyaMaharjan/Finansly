using Finansly.Application.DTOs.Categories;

namespace Finansly.Application.Services.Categories;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllByUserAsync(Guid userId);
    Task<CategoryDto?> GetByIdAsync(Guid id);
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
    Task UpdateAsync(Guid id, UpdateCategoryDto dto);
    Task DeleteAsync(Guid id);
}