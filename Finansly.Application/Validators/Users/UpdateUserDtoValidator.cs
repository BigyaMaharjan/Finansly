using Finansly.Application.DTOs.Users;
using FluentValidation;

namespace Finansly.Application.Validators.Users;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.DateOfBirth)
            .LessThan(_ => timeProvider.GetUtcNow().UtcDateTime).WithMessage("Date of birth must be in the past.")
            .When(x => x.DateOfBirth.HasValue);
    }
}
