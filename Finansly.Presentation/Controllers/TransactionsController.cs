using Finansly.Application.Common.Models;
using Finansly.Application.DTOs.Transactions;
using Finansly.Application.Services.Transactions;
using Finansly.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Finansly.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger)
    {
        _transactionService = transactionService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all transactions for a specific user.
    /// </summary>
    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TransactionDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<TransactionDto>>>> GetAllByUser(Guid userId)
    {
        var transactions = await _transactionService.GetAllByUserAsync(userId);
        return Ok(ApiResponse<IEnumerable<TransactionDto>>.Ok(transactions));
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
    /// Creates a new transaction.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TransactionDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<TransactionDto>>> Create([FromBody] CreateTransactionDto dto)
    {
        var created = await _transactionService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, ApiResponse<TransactionDto>.Created(created));
    }

    /// <summary>
    /// Updates an existing transaction.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object?>>> Update(Guid id, [FromBody] UpdateTransactionDto dto)
    {
        await _transactionService.UpdateAsync(id, dto);
        return Ok(ApiResponse<object?>.Ok(null, "Transaction updated successfully."));
    }

    /// <summary>
    /// Soft-deletes a transaction by its ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object?>>> Delete(Guid id)
    {
        await _transactionService.DeleteAsync(id);
        return Ok(ApiResponse<object?>.Ok(null, "Transaction deleted successfully."));
    }

    /// <summary>
    /// Gets the total transaction amount for a user by category type (Income/Expense), filtered by month and year.
    /// </summary>
    [HttpGet("user/{userId:guid}/summary")]
    [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<decimal>>> GetSummary(
        Guid userId,
        [FromQuery] CategoryType type,
        [FromQuery] int month,
        [FromQuery] int year)
    {
        if (month < 1 || month > 12)
            return BadRequest(ApiResponse<decimal>.Fail(400, "Month must be between 1 and 12."));

        if (year < 2000 || year > DateTime.UtcNow.Year + 1)
            return BadRequest(ApiResponse<decimal>.Fail(400, "Year is out of a valid range."));

        var total = await _transactionService.GetTotalByTypeAsync(userId, type, month, year);
        return Ok(ApiResponse<decimal>.Ok(total));
    }
}
