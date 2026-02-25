using Finansly.Application.DTOs.Categories;
using Finansly.Domain.Enums;
using FluentValidation;

namespace Finansly.Application.Validators.Categories;

public class CreateCategoryWithTransactionsDtoValidator : AbstractValidator<CreateCategoryWithTransactionsDto>
{
    public CreateCategoryWithTransactionsDtoValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage($"Type must be {CategoryType.Income} or {CategoryType.Expense}.");

        RuleFor(x => x.Transactions)
            .NotEmpty().WithMessage("At least one transaction is required.");

        RuleForEach(x => x.Transactions).SetValidator(new CreateTransactionForCategoryDtoValidator(timeProvider));
    }
}

public class CreateTransactionForCategoryDtoValidator : AbstractValidator<CreateTransactionForCategoryDto>
{
    public CreateTransactionForCategoryDtoValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.")
            .LessThanOrEqualTo(_ => timeProvider.GetUtcNow().UtcDateTime.AddDays(1)).WithMessage("Date cannot be in the future.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}
