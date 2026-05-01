using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.User.Login;
using Application.Features.Auth.Commands.User.Logout;
using Application.Features.Auth.Commands.User.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Created() : BadRequest(result.Error);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout(LogoutCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshToken(RefreshTokenCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}