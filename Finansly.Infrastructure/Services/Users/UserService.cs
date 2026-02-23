using Finansly.Application.DTOs.Users;
using Finansly.Application.Interfaces.Users;
using Finansly.Application.Services.Users;
using Finansly.Domain.Entities;

namespace Finansly.Infrastructure.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        return user is null ? null : MapToDto(user);
    }

    public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user is null)
            throw new KeyNotFoundException($"User with id {id} not found.");

        user.Name = dto.Name;
        if (dto.DateOfBirth.HasValue)
            user.DateOfBirth = dto.DateOfBirth.Value;
        user.Bio = dto.Bio;

        _repository.Update(user);
        await _repository.SaveChangesAsync();

        return MapToDto(user);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user is null)
            return false;

        _repository.Delete(user);
        await _repository.SaveChangesAsync();

        return true;
    }

    private static UserDto MapToDto(User u) => new()
    {
        Id = u.Id,
        Name = u.Name,
        Email = u.Email,
        DateOfBirth = u.DateOfBirth,
        Bio = u.Bio,
        CreatedAt = u.CreatedAt
    };
}
