using Finansly.Application.Common.Models;
using Finansly.Application.DTOs.Categories;
using Finansly.Application.Services.Categories;
using Microsoft.AspNetCore.Mvc;

namespace Finansly.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all categories for a specific user.
    /// </summary>
    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetAllByUser(Guid userId)
    {
        var categories = await _categoryService.GetAllByUserAsync(userId);
        return Ok(ApiResponse<IEnumerable<CategoryDto>>.Ok(categories));
    }

    /// <summary>
    /// Gets a single category by its ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> GetById(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category is null)
        {
            _logger.LogWarning("Category {Id} was not found", id);
            return NotFound(ApiResponse<CategoryDto>.Fail(404, $"Category with id {id} was not found."));
        }

        return Ok(ApiResponse<CategoryDto>.Ok(category));
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<Dictionary<string, string[]>>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<Guid>>> Create([FromBody] CreateCategoryDto dto)
    {
        var id = await _categoryService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, ApiResponse<Guid>.Ok(id));
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<Dictionary<string, string[]>>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<Guid>>> Update(Guid id, [FromBody] UpdateCategoryDto dto)
    {
        var updatedId = await _categoryService.UpdateAsync(id, dto);
        return Ok(ApiResponse<Guid>.Ok(updatedId, "Category updated successfully."));
    }

    /// <summary>
    /// Soft-deletes a category by its ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
    {
        var result = await _categoryService.DeleteAsync(id);
        if (!result)
            return NotFound(ApiResponse<bool>.Fail(404, $"Category with id {id} not found."));

        return Ok(ApiResponse<bool>.Ok(result, "Category deleted successfully."));
    }

    /// <summary>
    /// Creates a category together with its initial transactions in a single atomic operation.
    /// </summary>
    [HttpPost("with-transactions")]
    [ProducesResponseType(typeof(ApiResponse<CategoryWithTransactionsResultDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<Dictionary<string, string[]>>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<CategoryWithTransactionsResultDto>>> CreateWithTransactions(
        [FromBody] CreateCategoryWithTransactionsDto dto)
    {
        var result = await _categoryService.CreateWithTransactionsAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.CategoryId },
            ApiResponse<CategoryWithTransactionsResultDto>.Created(result));
    }
}
