using Finansly.Application.DTOs.Categories;
using Finansly.Application.Validators.Categories;
using FluentValidation.TestHelper;

namespace Finansly.Tests.Validators;

public class CreateCategoryDtoValidatorTests
{
    private readonly CreateCategoryDtoValidator _validator;

    public CreateCategoryDtoValidatorTests()
    {
        _validator = new CreateCategoryDtoValidator();
    }

    [Fact]
    public void Validate_ValidDto_ShouldPass()
    {
        var dto = new CreateCategoryDto
        {
            Name = "Salary",
            Type = Domain.Enums.CategoryType.Income
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    public void Validate_EmptyName_ShouldFail(string? name)
    {
        var dto = new CreateCategoryDto
        {
            Name = name!,
            Type = Domain.Enums.CategoryType.Income
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_NameTooLong_ShouldFail()
    {
        var dto = new CreateCategoryDto
        {
            Name = new string('a', 101),
            Type = Domain.Enums.CategoryType.Income
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
}
