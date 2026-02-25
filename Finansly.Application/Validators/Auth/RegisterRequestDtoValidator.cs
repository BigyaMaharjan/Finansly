using Finansly.Application.DTOs.Auth;
using FluentValidation;

namespace Finansly.Application.Validators.Auth;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestDtoValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

        RuleFor(x => x.DateOfBirth)
            .NotEqual(default(DateTime)).WithMessage("Date of birth is required.")
            .LessThan(_ => timeProvider.GetUtcNow().UtcDateTime).WithMessage("Date of birth must be in the past.");
    }
}
