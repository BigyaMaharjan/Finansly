using Finansly.Application.DTOs.Categories;
using Finansly.Domain.Enums;
using FluentValidation;

namespace Finansly.Application.Validators.Categories;

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage($"Type must be {CategoryType.Income} or {CategoryType.Expense}.");
    }
}
