using FluentValidation;

namespace Application.Features.Finance.Command.Account.PatchAccount;

public class PatchAccountCommandValidator : AbstractValidator<PatchAccountCommand>
{
    public PatchAccountCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
        
        RuleFor(x => x.Type)
            .MaximumLength(20).WithMessage("Type cannot be longer than 20 characters.");
    }
}
