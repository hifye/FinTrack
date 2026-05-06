using Application.Features.Finance.Command.Account.CreateAccount;
using Application.Features.Finance.Command.Account.DeleteAccount;
using Application.Features.Finance.Command.Account.PatchAccount;
using Application.Features.Finance.Command.RecurringTransaction.CreateRecurringTransaction;
using Application.Features.Finance.Command.RecurringTransaction.DeleteRecurringTransaction;
using Application.Features.Finance.Command.RecurringTransaction.PatchRecurringTransaction;
using Application.Features.Finance.Command.Transaction.CreateTransaction;
using Application.Features.Finance.Command.Transaction.DeleteTransaction;
using Application.Features.Finance.Command.Transaction.PatchTransaction;
using Application.Features.Finance.ListItem;
using Application.Features.Finance.Queries.Account.GetAccountDetails;
using Application.Features.Finance.Queries.Account.GetAccountsByUserId;
using Application.Features.Finance.Queries.RecurringTransaction.GetRecurringTransactionDetails;
using Application.Features.Finance.Queries.RecurringTransaction.GetRecurringTransactionsByUserId;
using Application.Features.Finance.Queries.Transaction.GetTransactionDetails;
using Application.Features.Finance.Queries.Transaction.GetTransactionsByUserId;
using Application.Features.Finance.Queries.Transaction.GetTransactionSummary;
using FinTrack.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers.Finance;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class FinanceController(IMediator mediator) : ControllerBase
{

    [ProducesResponseType(typeof(AccountListItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]   
    [HttpGet("account/{id:guid}")]
    public async Task<ActionResult<AccountListItem>> GetAccountDetails(Guid id)
    {
        var result = await mediator.Send(new GetAccountDetailsQuery(id));
        return result.ToActionResult();
    }

    [ProducesResponseType(typeof(AccountListItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]   
    [HttpGet("accounts")]
    public async Task<ActionResult<IReadOnlyList<AccountListItem>>> GetAccountsByUserId()
    {
        var result = await mediator.Send(new GetAccountsByUserIdQuery());
        return result.ToActionResult();
    }

    [ProducesResponseType(typeof(RecurringTransactionListItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]  
    [HttpGet("recurring-transaction/{id:guid}")]
    public async Task<ActionResult<RecurringTransactionListItem>> GetRecurringTransactionDetails(Guid id)
    {
        var result = await mediator.Send(new GetRecurringTransactionDetailsQuery(id));
        return result.ToActionResult();
    }
    
    [ProducesResponseType(typeof(RecurringTransactionListItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]  
    [HttpGet("recurring-transactions")]
    public async Task<ActionResult<IReadOnlyList<RecurringTransactionListItem>>> GetRecurringTransactionsByUserId()
    {
        var result = await mediator.Send(new GetRecurringTransactionsByUserIdQuery());
        return result.ToActionResult();
    }

    [ProducesResponseType(typeof(TransactionListItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet("transaction/{id:guid}")]
    public async Task<ActionResult<TransactionListItem>> GetTransactionDetails(Guid id)
    {
        var result = await mediator.Send(new GetTransactionDetailsQuery(id));
        return result.ToActionResult();
    }

    [ProducesResponseType(typeof(TransactionListItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
    [HttpGet("transactions")]
    public async Task<ActionResult<IReadOnlyList<TransactionListItem>>> GetTransactionsByUserId()
    {
        var result = await mediator.Send(new GetTransactionsByUserIdQuery());
        return result.ToActionResult();
    }

    [ProducesResponseType(typeof(TransactionSummary), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet("transaction-summary")]
    public async Task<ActionResult<TransactionSummary>> GetTransactionSummary([FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var result = await mediator.Send(new GetTransactionSummaryQuery(startDate, endDate));
        return result.ToActionResult();
    }
    
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("account")]
    public async Task<ActionResult> CreateAccount(CreateAccountCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailure)
            return result.ToActionResult();

        return CreatedAtAction(
            nameof(GetAccountDetails),
            new { id = result.Value },
            result.Value
        );
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPatch("account/{id:guid}")]
    public async Task<ActionResult> PatchAccount(Guid id, PatchAccountCommand command)
    {
        command = command with { Id = id };
        var result = await mediator.Send(command);
        return result.ToActionResult(StatusCodes.Status204NoContent);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpDelete("account/{id:guid}")]
    public async Task<ActionResult> DeleteAccount(Guid id)
    {
        var result = await mediator.Send(new DeleteAccountCommand(id));
        return result.ToActionResult(StatusCodes.Status204NoContent);
    }

    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("recurring-transaction")]
    public async Task<ActionResult> CreateRecurringTransaction(CreateRecurringTransactionCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailure)
            return result.ToActionResult();

        return CreatedAtAction(
            nameof(GetRecurringTransactionDetails),
            new { id = result.Value },
            result.Value
        );
    }
    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPatch("recurring-transaction/{id:guid}")]
    public async Task<ActionResult> PatchRecurringTransaction(Guid id, PatchRecurringTransactionCommand command)
    {
        command = command with { Id = id };
        var result = await mediator.Send(command);
        return result.ToActionResult(StatusCodes.Status204NoContent);
    }
    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpDelete("recurring-transaction/{id:guid}")]
    public async Task<ActionResult> DeleteRecurringTransaction(Guid id)
    {
        var result = await mediator.Send(new DeleteRecurringTransactionCommand(id));
        return result.ToActionResult(StatusCodes.Status204NoContent);
    }

    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("transaction")]
    public async Task<ActionResult> CreateTransaction(CreateTransactionCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailure)
            return result.ToActionResult();

        return CreatedAtAction(
            nameof(GetTransactionDetails),
            new { id = result.Value },
            result.Value
        );
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPatch("transaction/{id:guid}")]
    public async Task<ActionResult> PatchTransaction(Guid id, PatchTransactionCommand command)
    {
        command = command with { Id = id };
        var result = await mediator.Send(command);
        return result.ToActionResult(StatusCodes.Status204NoContent);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpDelete("transaction/{id:guid}")]
    public async Task<ActionResult> DeleteTransaction(Guid id)
    {
        var result = await mediator.Send(new DeleteTransactionCommand(id));
        return result.ToActionResult(StatusCodes.Status204NoContent);
    }
}