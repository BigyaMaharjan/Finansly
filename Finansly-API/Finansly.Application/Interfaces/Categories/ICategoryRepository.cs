using Finansly.Application.Common.Interfaces;
using Finansly.Domain.Entities;

namespace Finansly.Application.Interfaces.Categories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<IEnumerable<Category>> GetByUserAsync(Guid userId);
}