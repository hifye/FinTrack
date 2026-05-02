using Application.Features.Catalog.Commands.Category.CreateCategory;
using Application.Features.Catalog.Commands.Category.DeleteCategory;
using Application.Features.Catalog.Commands.Category.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers.Catalog;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CategoryController(IMediator mediator) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("Create")]
    public async Task<ActionResult> CreateCategory(CreateCategoryCommand command)
    {
        var result  = await mediator.Send(command);
        return result.IsSuccess ? Created() : BadRequest(result.Error);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("Update")]
    public async Task<ActionResult> UpdateCategory(UpdateCategoryCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("Delete")]
    public async Task<ActionResult> DeleteCategory(DeleteCategoryCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}