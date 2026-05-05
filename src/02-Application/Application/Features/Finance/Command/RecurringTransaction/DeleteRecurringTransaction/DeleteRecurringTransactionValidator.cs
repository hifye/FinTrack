using FluentValidation;

namespace Application.Features.Finance.Command.RecurringTransaction.DeleteRecurringTransaction;

public class DeleteRecurringTransactionValidator : AbstractValidator<DeleteRecurringTransactionCommand>
{
    public DeleteRecurringTransactionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
    }
}