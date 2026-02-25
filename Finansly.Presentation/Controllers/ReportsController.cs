using System.Security.Claims;
using Finansly.Application.Common.Models;
using Finansly.Application.DTOs.Reports;
using Finansly.Application.Services.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finansly.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly TimeProvider _timeProvider;

    public ReportsController(IReportService reportService, TimeProvider timeProvider)
    {
        _reportService = reportService;
        _timeProvider = timeProvider;
    }

    /// <summary>
    /// Returns total income, expense, and balance for the authenticated user in a given month and year.
    /// </summary>
    [HttpGet("monthly-summary")]
    [ProducesResponseType(typeof(ApiResponse<MonthlySummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<MonthlySummaryDto>>> GetMonthlySummary(
        [FromQuery] int month, [FromQuery] int year, CancellationToken cancellationToken)
    {
        if (month < 1 || month > 12)
            return BadRequest(ApiResponse<MonthlySummaryDto>.Fail(400, "Month must be between 1 and 12."));

        if (year < 2000 || year > _timeProvider.GetUtcNow().Year + 1)
            return BadRequest(ApiResponse<MonthlySummaryDto>.Fail(400, "Year is out of a valid range."));

        var userId = GetCurrentUserId();
        var result = await _reportService.GetMonthlySummaryAsync(userId, month, year, cancellationToken);
        return Ok(ApiResponse<MonthlySummaryDto>.Ok(result));
    }

    /// <summary>
    /// Returns the total spent per category for the authenticated user in a given month and year.
    /// </summary>
    [HttpGet("category-breakdown")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryBreakdownDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoryBreakdownDto>>>> GetCategoryBreakdown(
        [FromQuery] int month, [FromQuery] int year, CancellationToken cancellationToken)
    {
        if (month < 1 || month > 12)
            return BadRequest(ApiResponse<IEnumerable<CategoryBreakdownDto>>.Fail(400, "Month must be between 1 and 12."));

        if (year < 2000 || year > _timeProvider.GetUtcNow().Year + 1)
            return BadRequest(ApiResponse<IEnumerable<CategoryBreakdownDto>>.Fail(400, "Year is out of a valid range."));

        var userId = GetCurrentUserId();
        var result = await _reportService.GetCategoryBreakdownAsync(userId, month, year, cancellationToken);
        return Ok(ApiResponse<IEnumerable<CategoryBreakdownDto>>.Ok(result));
    }

    /// <summary>
    /// Returns the all-time total income, expense, and running balance for the authenticated user.
    /// </summary>
    [HttpGet("balance")]
    [ProducesResponseType(typeof(ApiResponse<MonthlySummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<MonthlySummaryDto>>> GetBalance(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var result = await _reportService.GetBalanceAsync(userId, cancellationToken);
        return Ok(ApiResponse<MonthlySummaryDto>.Ok(result));
    }

    private Guid GetCurrentUserId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");
        return Guid.Parse(sub!);
    }
}
