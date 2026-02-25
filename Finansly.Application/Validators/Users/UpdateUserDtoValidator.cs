using Finansly.Application.DTOs.Users;
using Finansly.Domain.Constants;
using FluentValidation;

namespace Finansly.Application.Validators.Users;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(EntityLengths.User.Name).WithMessage($"Name must not exceed {EntityLengths.User.Name} characters.");

        RuleFor(x => x.DateOfBirth)
            .LessThan(_ => timeProvider.GetUtcNow().UtcDateTime).WithMessage("Date of birth must be in the past.")
            .When(x => x.DateOfBirth.HasValue);
    }
}
