using FluentValidation;

namespace Application.Features.Finance.Command.Account.CreateAccount;

public class CreateAccountValidator : AbstractValidator<CreateAccount.CreateAccountCommand>
{
    public CreateAccountValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Account name cannot be empty")
            .MaximumLength(50)
            .WithMessage("Account name cannot be longer than 50 characters");
        
        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Account type cannot be empty")
            .MaximumLength(20)
            .WithMessage("Account type cannot be longer than 20 characters");
        
        RuleFor(x => x.InitialBalance)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Initial balance cannot be negative");
    }
}