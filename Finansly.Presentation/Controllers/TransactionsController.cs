using System.Security.Claims;
using Finansly.Application.Common.Models;
using Finansly.Application.DTOs.Transactions;
using Finansly.Application.Services.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finansly.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ILogger<TransactionsController> _logger;
    private readonly TimeProvider _timeProvider;

    public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger, TimeProvider timeProvider)
    {
        _transactionService = transactionService;
        _logger = logger;
        _timeProvider = timeProvider;
    }

    /// <summary>
    /// Gets a paginated, filtered, and sorted list of transactions for the authenticated user.
    /// Supports: skipCount, maxResultCount, sorting (date|amount|description), sortType (Ascending|Descending),
    /// categoryId, dateFrom, dateTo, type (Income|Expense).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResultDto<TransactionDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<Dictionary<string, string[]>>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<PagedResultDto<TransactionDto>>>> GetPaged(
        [FromQuery] GetTransactionsRequestDto request)
    {
        var userId = GetCurrentUserId();
        var result = await _transactionService.GetPagedAsync(userId, request);
        return Ok(ApiResponse<PagedResultDto<TransactionDto>>.Ok(result));
    }

    /// <summary>
    /// Gets a single transaction by its ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<TransactionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TransactionDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TransactionDto>>> GetById(Guid id)
    {
        var transaction = await _transactionService.GetByIdAsync(id);

        if (transaction is null)
        {
            _logger.LogWarning("Transaction {Id} was not found", id);
            return NotFound(ApiResponse<TransactionDto>.Fail(404, $"Transaction with id {id} was not found."));
        }

        return Ok(ApiResponse<TransactionDto>.Ok(transaction));
    }

    /// <summary>
    /// Creates a new transaction for the authenticated user.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<Dictionary<string, string[]>>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<Guid>>> Create([FromBody] CreateTransactionDto dto)
    {
        var userId = GetCurrentUserId();
        var id = await _transactionService.CreateAsync(userId, dto);
        return CreatedAtAction(nameof(GetById), new { id }, ApiResponse<Guid>.Ok(id));
    }

    /// <summary>
    /// Updates an existing transaction.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<Dictionary<string, string[]>>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<Guid>>> Update(Guid id, [FromBody] UpdateTransactionDto dto)
    {
        var userId = GetCurrentUserId();
        var updatedId = await _transactionService.UpdateAsync(id, userId, dto);
        return Ok(ApiResponse<Guid>.Ok(updatedId, "Transaction updated successfully."));
    }

    /// <summary>
    /// Soft-deletes a transaction by its ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
    {
        var result = await _transactionService.DeleteAsync(id);
        if (!result)
            return NotFound(ApiResponse<bool>.Fail(404, $"Transaction with id {id} not found."));

        return Ok(ApiResponse<bool>.Ok(result, "Transaction deleted successfully."));
    }

    /// <summary>
    /// Gets the total transaction amount for the authenticated user by category type (Income/Expense), filtered by month and year.
    /// </summary>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<decimal>>> GetSummary(
        [FromQuery] GetTotalByTypeRequestDto request)
    {
        if (request.Month < 1 || request.Month > 12)
            return BadRequest(ApiResponse<decimal>.Fail(400, "Month must be between 1 and 12."));

        if (request.Year < 2000 || request.Year > _timeProvider.GetUtcNow().Year + 1)
            return BadRequest(ApiResponse<decimal>.Fail(400, "Year is out of a valid range."));

        var userId = GetCurrentUserId();
        var total = await _transactionService.GetTotalByTypeAsync(userId, request);
        return Ok(ApiResponse<decimal>.Ok(total));
    }

    private Guid GetCurrentUserId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");
        return Guid.Parse(sub!);
    }
}
