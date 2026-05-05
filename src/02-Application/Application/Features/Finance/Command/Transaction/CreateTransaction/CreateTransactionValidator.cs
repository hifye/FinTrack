using FluentValidation;

namespace Application.Features.Finance.Command.Transaction.CreateTransaction;

public class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("Account id is required.");
        
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category id is required.");
        
        RuleFor(x => x.RecurringId)
            .NotEmpty()
            .WithMessage("Recurring id is required.");       
        
        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("Amount is required.");
        
        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Type is required.")
            .MaximumLength(100).WithMessage("Type cannot be longer than 100 characters.");
        
        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Description cannot be longer than 250 characters.");
    }
}