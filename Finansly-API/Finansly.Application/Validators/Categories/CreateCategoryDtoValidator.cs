using Finansly.Application.DTOs.Categories;
using Finansly.Domain.Constants;
using Finansly.Domain.Enums;
using FluentValidation;

namespace Finansly.Application.Validators.Categories;

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(EntityLengths.Category.Name).WithMessage($"Name cannot exceed {EntityLengths.Category.Name} characters.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage($"Type must be {CategoryType.Income} or {CategoryType.Expense}.");
    }
}
