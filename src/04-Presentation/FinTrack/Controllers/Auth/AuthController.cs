using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.User.Login;
using Application.Features.Auth.Commands.User.Logout;
using Application.Features.Auth.Commands.User.Register;
using Application.Features.Auth.Commands.User.UpdatePassword;
using FinTrack.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginCommand command)
    {
        var result = await mediator.Send(command);
        return result.ToActionResult();
    }
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterCommand command)
    {
        var result = await mediator.Send(command);
        return result.ToActionResult(StatusCodes.Status201Created);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    [HttpPatch("update")]
    public async Task<ActionResult> UpdatePassword(UpdatePasswordCommand command)
    {
        var result = await mediator.Send(command);
        return result.ToActionResult();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout(LogoutCommand command)
    {
        var result = await mediator.Send(command);
        return result.ToActionResult(StatusCodes.Status204NoContent);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshToken(RefreshTokenCommand command)
    {
        var result = await mediator.Send(command);
        return result.ToActionResult();
    }
}