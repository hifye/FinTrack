using FluentValidation;

namespace Application.Features.Finance.Command.RecurringTransaction.PatchRecurringTransaction;

public class PatchRecurringTransactionValidator : AbstractValidator<PatchRecurringTransactionCommand>
{
    public PatchRecurringTransactionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
        
        RuleFor(x => x.Type)
            .MaximumLength(100).WithMessage("Type cannot be longer than 100 characters.");
        
        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Description cannot be longer than 250 characters.");
        
        RuleFor(x => x.Frequency)
            .MaximumLength(50).WithMessage("Frequency cannot be longer than 50 characters.");       
    }
}