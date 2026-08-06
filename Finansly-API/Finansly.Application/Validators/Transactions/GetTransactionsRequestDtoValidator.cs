using Finansly.Application.DTOs.Transactions;
using FluentValidation;

namespace Finansly.Application.Validators.Transactions;

public class GetTransactionsRequestDtoValidator : AbstractValidator<GetTransactionsRequestDto>
{
    public GetTransactionsRequestDtoValidator()
    {
        RuleFor(x => x.SkipCount)
            .GreaterThanOrEqualTo(0).WithMessage("SkipCount must be 0 or greater.");

        RuleFor(x => x.MaxResultCount)
            .InclusiveBetween(1, 100).WithMessage("MaxResultCount must be between 1 and 100.");

        When(x => x.DateFrom.HasValue && x.DateTo.HasValue, () =>
        {
            RuleFor(x => x.DateFrom)
                .LessThanOrEqualTo(x => x.DateTo)
                .WithMessage("DateFrom must be earlier than or equal to DateTo.");
        });
    }
}
