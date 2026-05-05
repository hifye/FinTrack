using Application.Features.Catalog.Commands.Category.CreateCategory;
using Application.Features.Catalog.Commands.Category.DeleteCategory;
using Application.Features.Catalog.Commands.Category.PatchCategory;
using Application.Features.Catalog.ListItem;
using Application.Features.Catalog.Queries.Category.GetCategoriesByUserId;
using Application.Features.Catalog.Queries.Category.GetCategoryDetails;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers.Catalog;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CatalogController(IMediator mediator) : ControllerBase
{
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet("GetCategoryDetails")]
    public async Task<ActionResult<CategoryListItem>> GetCategoryDetails(Guid id)
    {
        var category = await mediator.Send(new GetCategoryDetailsQuery(id));
        return Ok(category);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet("GetCategoriesByUserId")]
    public async Task<ActionResult<IReadOnlyList<CategoryListItem>>> GetCategoriesByUserId(
        GetCategoriesByUserIdQuery query)
    {
        var categories = await mediator.Send(query);
        return Ok(categories);
    }
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("Create")]
    public async Task<ActionResult> CreateCategory(CreateCategoryCommand command)
    {
        var result  = await mediator.Send(command);
        return result.IsSuccess ? Created() : BadRequest(result.Error);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPatch("Patch")]
    public async Task<ActionResult> PatchCategory(PatchCategoryCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpDelete("Delete")]
    public async Task<ActionResult> DeleteCategory(DeleteCategoryCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}