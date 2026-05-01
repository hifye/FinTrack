using FluentValidation;

namespace Application.Features.Auth.Commands.User.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.")
            .MaximumLength(100).WithMessage("Email cannot be longer than 100 characters.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}