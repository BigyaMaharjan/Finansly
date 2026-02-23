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
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> Create([FromBody] CreateCategoryDto dto)
    {
        var created = await _categoryService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, ApiResponse<CategoryDto>.Created(created));
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object?>>> Update(Guid id, [FromBody] UpdateCategoryDto dto)
    {
        await _categoryService.UpdateAsync(id, dto);
        return Ok(ApiResponse<object?>.Ok(null, "Category updated successfully."));
    }

    /// <summary>
    /// Soft-deletes a category by its ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object?>>> Delete(Guid id)
    {
        await _categoryService.DeleteAsync(id);
        return Ok(ApiResponse<object?>.Ok(null, "Category deleted successfully."));
    }
}
