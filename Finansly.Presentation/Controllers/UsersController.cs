using System.Security.Claims;
using Finansly.Application.Common.Models;
using Finansly.Application.DTOs.Users;
using Finansly.Application.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finansly.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Gets the authenticated user's profile.
    /// </summary>
    [HttpGet("me")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetMe()
    {
        var userId = GetCurrentUserId();
        var user = await _userService.GetByIdAsync(userId);
        if (user is null)
        {
            _logger.LogWarning("User {Id} was not found", userId);
            return NotFound(ApiResponse<UserDto>.Fail(404, $"User with id {userId} was not found."));
        }

        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    /// <summary>
    /// Updates the authenticated user's profile (name, date of birth, bio).
    /// </summary>
    [HttpPut("me")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<Dictionary<string, string[]>>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateMe([FromBody] UpdateUserDto dto)
    {
        var userId = GetCurrentUserId();
        var user = await _userService.UpdateAsync(userId, dto);
        return Ok(ApiResponse<UserDto>.Ok(user, "Profile updated successfully."));
    }

    /// <summary>
    /// Soft-deletes the authenticated user's account.
    /// </summary>
    [HttpDelete("me")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteMe()
    {
        var userId = GetCurrentUserId();
        var result = await _userService.DeleteAsync(userId);
        if (!result)
            return NotFound(ApiResponse<bool>.Fail(404, $"User with id {userId} not found."));

        return Ok(ApiResponse<bool>.Ok(result, "Account deleted successfully."));
    }

    private Guid GetCurrentUserId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");
        return Guid.Parse(sub!);
    }
}
