using FluentValidation;

namespace Application.Features.Finance.Command.Transaction.PatchTransaction;

public class PatchTransactionCommandValidator : AbstractValidator<PatchTransactionCommand>
{
    public PatchTransactionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
        
        RuleFor(x => x.Type)
            .MaximumLength(100).WithMessage("Type cannot be longer than 100 characters.");
        
        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Description cannot be longer than 250 characters.");       
    }
}