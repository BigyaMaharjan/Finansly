using Finansly.Application.Common.Interfaces;
using Finansly.Domain.Entities;

namespace Finansly.Application.Interfaces.Users;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsEmailAsync(string email);
}
