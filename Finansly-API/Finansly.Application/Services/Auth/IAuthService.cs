using Finansly.Application.DTOs.Auth;

namespace Finansly.Application.Services.Auth;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto);
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
}
