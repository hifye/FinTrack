using Application.Features.Finance.Command.Account.CreateAccount;
using Application.Features.Finance.Command.Account.DeleteAccount;
using Application.Features.Finance.Command.Account.PatchAccount;
using Application.Features.Finance.Command.RecurringTransaction.CreateRecurringTransaction;
using Application.Features.Finance.Command.RecurringTransaction.DeleteRecurringTransaction;
using Application.Features.Finance.Command.RecurringTransaction.PatchRecurringTransaction;
using Application.Features.Finance.Command.Transaction.CreateTransaction;
using Application.Features.Finance.Command.Transaction.DeleteTransaction;
using Application.Features.Finance.Command.Transaction.PatchTransaction;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers.Finance;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class FinanceController(IMediator mediator) : Controller
{
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("CreateAccount")]
    public async Task<ActionResult> CreateAccount(CreateAccountCommand command)
    {
        var result  = await mediator.Send(command);
        return result.IsSuccess ? Created() : BadRequest(result.Error);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPatch("PatchAccount")]
    public async Task<ActionResult> PatchAccount(PatchAccountCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok() : NotFound(result.Error);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpDelete("DeleteAccount")]
    public async Task<ActionResult> DeleteAccount(DeleteAccountCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("CreateRecurringTransaction")]
    public async Task<ActionResult> CreateRecurringTransaction(CreateRecurringTransactionCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Created() : BadRequest(result.Error);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPatch("PatchRecurringTransaction")]
    public async Task<ActionResult> PatchRecurringTransaction(PatchRecurringTransactionCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok() : NotFound(result.Error);
    }
    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpDelete("DeleteRecurringTransaction")]
    public async Task<ActionResult> DeleteRecurringTransaction(DeleteRecurringTransactionCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("CreateTransaction")]
    public async Task<ActionResult> CreateTransaction(CreateTransactionCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Created() : BadRequest(result.Error);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPatch("PatchTransaction")]
    public async Task<ActionResult> PatchTransaction(PatchTransactionCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpDelete("DeleteTransaction")]
    public async Task<ActionResult> DeleteTransaction(DeleteTransactionCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }
}