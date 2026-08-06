using Finansly.Application.Common.Models;
using Finansly.Application.DTOs.Auth;
using Finansly.Application.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Finansly.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (result is null)
        {
            _logger.LogWarning("Failed login attempt for {Email}", dto.Email);
            return Unauthorized(ApiResponse<AuthResponseDto>.Fail(401, "Invalid email or password."));
        }

        _logger.LogInformation("User {Email} logged in successfully", dto.Email);
        return Ok(ApiResponse<AuthResponseDto>.Ok(result));
    }

    /// <summary>
    /// Registers a new user and returns the user ID.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<Dictionary<string, string[]>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<Guid>>> Register([FromBody] RegisterRequestDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        _logger.LogInformation("New user registered with email {Email}", dto.Email);
        return StatusCode(StatusCodes.Status201Created, ApiResponse<Guid>.Created(result.UserId));
    }
}
